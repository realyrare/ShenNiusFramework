using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ShenNius.Share.Infrastructure.Configurations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

/*************************************
* 类名：RabbitMQHelper
* 作者：realyrare
* 邮箱：mhg215@yeah.net
* 时间：2021/8/6 15:56:40
*┌───────────────────────────────────┐　    
*│　   版权所有：神牛软件　　　　	     │
*└───────────────────────────────────┘
**************************************/

namespace ShenNius.Share.Infrastructure.MsgQueue
{
    public class RabbitMQHelper
    {
        private readonly ConnectionFactory factory;
        public RabbitMQHelper()
        {
            //创建连接工厂
            factory = new ConnectionFactory
            {
                HostName = AppSettings._config["RabbitMQ:HostName"],
                UserName = AppSettings._config["RabbitMQ:UserName"],
                Password = AppSettings._config["RabbitMQ:Password"]
            };
        }

        #region 主题发布订阅
        // direct（明确的路由规则：消费端绑定的队列名称必须和消息发布时指定的路由名称一致）
        // topic （模式匹配的路由规则：支持通配符）opic是direct的升级版，是一种模式匹配的路由机制。它支持使用两种通配符来进行模式匹配：符号#和符号*。其中*匹配一个单词， #则表示匹配0个或多个单词，单词之间用.分割
        //fanout （消息广播，将消息分发到exchange上绑定的所有队列上）
        public void Send(string msg, string type = null, string exchange = null, string routingKey = null, string queue = null)
        {
            if (string.IsNullOrEmpty(msg))
            {
                throw new ArgumentNullException("消息内容不能为空!");
            }
            var body = Encoding.UTF8.GetBytes(msg);
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //基本用法
                    if (string.IsNullOrEmpty(type))
                    {
                        //定义一个队列 
                        channel.QueueDeclare(queue, false, false, false, null);
                        channel.BasicPublish("", queue, null, body); //发送消息
                    }
                    //声明创建一个交换机，交换机类型设定为‘type指定的值’
                    channel.ExchangeDeclare(exchange: exchange, type: type);
                    if (type == "topic")
                    {
                        channel.BasicPublish(exchange, routingKey, null, body);    //发布消息
                    }
                    if (type == "fanout")
                    {
                        var queueName = channel.QueueDeclare().QueueName;
                        //发布到指定exchange，fanout类型无需指定routingKey
                        channel.BasicPublish(exchange: exchange, routingKey: "", basicProperties: null, body: body);
                    }
                    if (type == "direct")
                    {
                        //发布到direct类型exchange，必须指定routingKey
                        channel.BasicPublish(exchange: exchange, routingKey: routingKey, basicProperties: null, body: body);
                    }
                }
            }
        }
        public void Recive(string type, string exchange, string routingKey, string queue)
        {
            string queueName = queue;
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    if (string.IsNullOrEmpty(type))
                    {
                        //定义一个队列
                        channel.QueueDeclare(queue, false, false, false, null);
                    }

                    channel.ExchangeDeclare(exchange: exchange, type: type);
                    if (type == "topic")
                    {
                        //获取连接通道所使用的队列
                        queueName = channel.QueueDeclare().QueueName;
                        channel.QueueBind(queueName, exchange, routingKey: routingKey);   //队列绑定到交换机                       
                    }
                    if (type == "fanout")
                    {
                        //申明fanout类型exchange
                        channel.ExchangeDeclare(exchange: "fanoutEC", type: "fanout");
                        //申明随机队列名称
                        queueName = channel.QueueDeclare().QueueName;
                        //绑定队列到指定fanout类型exchange，无需指定路由键
                        channel.QueueBind(queue: queueName, exchange: "fanoutEC", routingKey: "");
                    }
                    if (type == "direct")
                    {
                        //申明direct类型exchange
                        channel.ExchangeDeclare(exchange: "directEC", type: "direct");
                        //绑定队列到direct类型exchange，需指定路由键routingKey
                        channel.QueueBind(queue: "green", exchange: "directEC", routingKey: "green");
                    }
                    if (type != "topic" || type != "fanout" || type != "direct")
                    {
                        throw new ArgumentNullException("指定的type匹配不到具体分支的值");
                    }
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (s, e) =>
                    {
                        var routingKey = e.RoutingKey;
                        var body = e.Body;
                        var message = Encoding.UTF8.GetString(body.Span);
                    };
                    //开启消费者与通道、队列关联
                    channel.BasicConsume(queueName, true, consumer);
                }
            }
        }

        #endregion

    }


}