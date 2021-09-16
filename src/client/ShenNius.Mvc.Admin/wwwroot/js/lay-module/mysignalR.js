
var connection = new signalR.HubConnectionBuilder().withUrl("/userLoginNotifiHub").build();
connection.start().then(function () {
    console.log("连接成功");
}).catch(function (err) {
    return console.error(err.toString());
});

function SaveCurrentUserInfo (userId) {
connection.invoke("SaveCurrentUserInfo", userId, true).catch(function (err) {
                return console.error(err.toString());
            });       
};






