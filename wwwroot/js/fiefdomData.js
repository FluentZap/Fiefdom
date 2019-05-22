var connection = new signalR.HubConnectionBuilder().withUrl("/fiefdomHub").build();


var fief = {};

fief.plots = [];
fief.resources = {};
fief.gameState = {};
fief.market = {};


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
};

function UpdateFiefdom() {
	console.log("here");
	connection.invoke("RequestFiefdomData").catch(function (err) {
    return console.error(err.toString());
  });
};

function BuyResource(type, quantity) {
	console.log("bought");
	connection.invoke("BuyResource", type, quantity).catch(function (err) {
    return console.error(err.toString());
  });
};

function GetMarketPrice() {
	connection.invoke("GetMarketPrice").catch(function (err) {
    return console.error(err.toString());
  });
};

function BuildPlot(id, type) {
	connection.invoke("BuildPlot", id, type).catch(function (err) {
		return console.error(err.toString());
	});
};

connection.on("ReceiveMarketPrices", function (prices) {
	prices.forEach(function(x){
		market[x.type] = x.price;
	});
});

function SellResource(type, quantity) {
	connection.invoke("SellResource", type, quantity).catch(function (err) {
    return console.error(err.toString());
  });
};

connection.on("RecieveFiefdomData", function (plots, gameState, market) {
	
	if (plots == null) {
		window.location.href = "/";
	}
	//Plots
	for (var i = 0; i < plots.fiefdomPlot.length; i++) {
		fief.plots[i] = plots.fiefdomPlot[i].type;
	}
	//Resources
  plots.fiefdomResources.forEach(function(p)
  {
    fief.resources[p.type] = p.quantity;
	});
	fief.title = plots.title;

	fief.gameState = gameState;
	fief.market = market;

	plots.fiefdomResources.forEach(function (p) {
		fief.resources[p.type] = p.quantity;
	});




	if (initialStart === false)
	{
		game.scene.run('scene');
		initialStart = true;
	}
});
