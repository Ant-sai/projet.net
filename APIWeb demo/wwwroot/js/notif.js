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
        if (currentState < 5) {
            changeState(currentState + 1)
        }
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


/* card alerte */

let cardAlert = document.getElementById("alerte-sonnette")
let textAlert = document.getElementById("text-alert")
let currentState = 0
let statePhrase = [
    "Nobody's here",
    "Toc toc",
    "Someone's waiting",
    "Hey, hurry up",
    "That's not realy nice to leave someone outside",
    "M*********r, will you open this g*d d**n door !!!"
]

function changeState(newState) {
    console.log("change state attempt " + newState)
    console.log(cardAlert)
    console.log("old state :" + currentState)
    cardAlert.classList.remove(`alerte-state-${currentState}`)
    cardAlert.classList.add(`alerte-state-${newState}`)
    textAlert.innerText = statePhrase[newState]
    currentState = newState
    console.log("new state :" + currentState)
}

document.getElementById("open").addEventListener("click", () => {
    changeState(0)
})

changeState(0)

