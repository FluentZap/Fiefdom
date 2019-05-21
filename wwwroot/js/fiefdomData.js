var connection = new signalR.HubConnectionBuilder().withUrl("/fiefdomHub").build();


var fief = {};

fief.plots = [];
fief.resources = {};

$(function () {
	connection.start().then(function () {
		UserLogin();
		console.log("Connected");
		UpdateFiefdom();
	});
});

function UserLogin() {	
	connection.invoke("UserLogin", userName).catch(function (err) {
		return console.error(err.toString());
	});
}

function UpdateFiefdom() {
	connection.invoke("RequestFiefdomData").catch(function (err) {
    return console.error(err.toString());
  });
}

function BuyResource(type, quantity) {
	connection.invoke("BuyResource", type, quantity).catch(function (err) {
    return console.error(err.toString());
  });
}

connection.on("RecieveFiefdomData", function (plots, resources) {
	if (plots == null) {
		window.location.href = "/";
	}
  for (var i = 0; i < plots.length; i++) {
    fief.plots[i] = plots[i].type;
	}
  resources.forEach(function(p)
  {
    fief.resources[p.type] = p.quantity;
	});
});