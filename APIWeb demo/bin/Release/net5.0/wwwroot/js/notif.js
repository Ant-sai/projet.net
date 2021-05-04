"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/notifHub", {
    skipNegotiation: true,
    transport: signalR.HttpTransportType.WebSockets
}).build();

connection.on("ReceiveNotif", function (message) {
    console.log("begin of fct")
    console.log(message)
    let date = new Date()
    var li = document.createElement("li");
    console.log("li and date created")
    if (message) {
        console.log("sonnette active")
        li.textContent = `La sonnette a été appuyé à ${date.toLocaleTimeString()}`;
        document.getElementById("sonnette").classList.add("sonnette-active")
    }
    else {
        console.log("sonnette inactive")
        li.textContent = `La sonnette a été relaché à ${date.toLocaleTimeString()}`;
        document.getElementById("sonnette").classList.remove("sonnette-active")
    }
    console.log("insert od li")
    document.getElementById("notifList").appendChild(li);
    console.log("end of fct")
});

connection.start()
    .catch(function (err) {
        return console.error(err.toString());
    });