var connection = new signalR.HubConnectionBuilder().withUrl("/fiefdomHub").build();


// function fiefType () {
//   this.plots = [],
//   this.resources = {},
// }

// function plot(){
//   fief.add(this);
// }
var fief = {};
fief.plots = [];
fief.resources = {};

$(function () {

  connection.start().then(function () {
    SendData();
    console.log("connected");
  });

});

function SendData() {
  connection.invoke("RequestFiefdomData").catch(function (err) {
    return console.error(err.toString());
  });
}

connection.on("RecieveFiefdomData", function (plots, resources) {
  for (var i = 0; i < plots.length; i++) {
    fief.plots[i] = plots[i].type;
  }
  resources.forEach(function(p)
  {
    fief.resources[p.type] = p.quantity;
  });
});

function GetFiefdomData() {

}


// "use strict";


//Disable send button until connection is established
//document.getElementById("sendButton").disabled = true;

// connection.on("ReceiveMessage", function (user, message) {
  // 	var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
  // 	var encodedMsg = user + " says " + msg;
  // 	var li = document.createElement("li");
  // 	li.textContent = encodedMsg;
  // 	document.getElementById("messagesList").appendChild(li);
  // });
  //
  // connection.start().then(function () {
    //
    // 	document.getElementById("sendButton").disabled = false;
    // }).catch(function (err) {
      // 	return console.error(err.toString());
      //
      // });
      //
      // document.getElementById("sendButton").addEventListener("click", function (event) {
        // 	var user = document.getElementById("userInput").value;
        // 	var message = document.getElementById("messageInput").value;
        // 	connection.invoke("SendMessage", user, message).catch(function (err) {
          // 		return console.error(err.toString());
          // 	});
          // 	event.preventDefault();
          // });
