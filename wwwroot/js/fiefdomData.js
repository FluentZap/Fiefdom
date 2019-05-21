var connection = new signalR.HubConnectionBuilder().withUrl("/fiefdomHub").build();


var fief = {};

fief.plots = [];
fief.resources = {};

var initialStart = false;
var market = {};

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

function GetMarketPrice() {
	connection.invoke("GetMarketPrice").catch(function (err) {
    return console.error(err.toString());
  });
}

connection.on("ReceiveMarketPrices", function (prices) {
	prices.forEach(function(x){
		market[x.type] = x.price;
	});
});

function SellResource(type, quantity) {
	connection.invoke("SellResource", type, quantity).catch(function (err) {
    return console.error(err.toString());
  });
}

connection.on("RecieveFiefdomData", function (plots, resources, title) {
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
	fief.title = title;
	if (initialStart === false)
	{
		game.scene.run('scene');
		initialStart = true;
	}
});
