﻿/*************************************
* 类名：EventBus
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/3 19:53:49
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace ShenNius.Share.Infrastructure.EventBus
{
    public class EventBus
    {
        public static EventBus Default => new EventBus();

        /// <summary>
        /// 定义线程安全集合
        /// </summary>
        private readonly ConcurrentDictionary<Type, List<Type>> _eventAndHandlerMapping;

        public EventBus()
        {
            _eventAndHandlerMapping = new ConcurrentDictionary<Type, List<Type>>();
            MapEventToHandler();
        }

        /// <summary>
        ///通过反射，将事件源与事件处理绑定
        /// </summary>
        private void MapEventToHandler()
        {
            Assembly assembly = Assembly.GetEntryAssembly();
            foreach (var type in assembly.GetTypes())
            {
                if (typeof(IEventHandler).IsAssignableFrom(type))//判断当前类型是否实现了IEventHandler接口
                {
                    Type handlerInterface = type.GetInterface("IEventHandler`1");//获取该类实现的泛型接口
                    if (handlerInterface != null)
                    {
                        Type eventDataType = handlerInterface.GetGenericArguments()[0]; // 获取泛型接口指定的参数类型

                        if (_eventAndHandlerMapping.ContainsKey(eventDataType))
                        {
                            List<Type> handlerTypes = _eventAndHandlerMapping[eventDataType];
                            handlerTypes.Add(type);
                            _eventAndHandlerMapping[eventDataType] = handlerTypes;
                        }
                        else
                        {
                            var handlerTypes = new List<Type> { type };
                            _eventAndHandlerMapping[eventDataType] = handlerTypes;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 手动绑定事件源与事件处理
        /// </summary>
        /// <typeparam name="TEventData"></typeparam>
        /// <param name="eventHandler"></param>
        public void Register<TEventData>(Type eventHandler)
        {
            List<Type> handlerTypes = _eventAndHandlerMapping[typeof(TEventData)];
            if (!handlerTypes.Contains(eventHandler))
            {
                handlerTypes.Add(eventHandler);
                _eventAndHandlerMapping[typeof(TEventData)] = handlerTypes;
            }
        }

        /// <summary>
        /// 手动解除事件源与事件处理的绑定
        /// </summary>
        /// <typeparam name="TEventData"></typeparam>
        /// <param name="eventHandler"></param>
        public void UnRegister<TEventData>(Type eventHandler)
        {
            List<Type> handlerTypes = _eventAndHandlerMapping[typeof(TEventData)];
            if (handlerTypes.Contains(eventHandler))
            {
                handlerTypes.Remove(eventHandler);
                _eventAndHandlerMapping[typeof(TEventData)] = handlerTypes;
            }
        }

        /// <summary>
        /// 根据事件源触发绑定的事件处理
        /// </summary>
        /// <typeparam name="TEventData"></typeparam>
        /// <param name="eventData"></param>
        public void Trigger<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            List<Type> handlers = _eventAndHandlerMapping[eventData.GetType()];

            if (handlers != null && handlers.Count > 0)
            {
                foreach (var handler in handlers)
                {
                    MethodInfo methodInfo = handler.GetMethod("HandleEvent");
                    if (methodInfo != null)
                    {
                        object obj = Activator.CreateInstance(handler);
                        methodInfo.Invoke(obj, new object[] { eventData });
                    }
                }
            }
        }
    }
}