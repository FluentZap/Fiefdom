var connection = new signalR.HubConnectionBuilder().withUrl("/fiefdomHub").build();

$(function () {
  connection.start().then(function () {
    console.log("connected");
  });
});


document.getElementById("login").addEventListener("click", function (event) {
  var name = document.getElementById("userInput").value;
  connection.invoke("UserLogin", name).catch(function (err) {
    return console.error(err.toString());
  });
  event.preventDefault();
});


connection.on("LoginUser", function () {
  window.location.href = "/index";
});
