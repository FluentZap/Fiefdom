var connection = new signalR.HubConnectionBuilder().withUrl("/fiefdomHub").build();


var fief = {};

fief.plots = [];
fief.resources = {};
fief.gameState = {};
fief.baseMarket = [];
fief.buyMarket = [];
fief.sellMarket = [];

fief.ballots = [];
fief.tax = 0;
fief.edicts = [{ type: "Empty", Amount: 0 }, { type: "Empty", Amount: 0 }, { type: "Empty", Amount: 0 }];
fief.vote = ["vote", "vote", "vote"];

var initialStart = false;
var market = {};

$(function () {
	connection.start().then(function () {
		UserLogin();
		console.log("Connected");
		UpdateFiefdom();
	});
});


function SubmitVote(ballot, vote) {
	//console.log(ballot);
	connection.invoke("SubmitVote", ballot, vote).catch(function (err) {
		return console.error(err.toString());
	});
};

function UserLogin() {
	connection.invoke("UserLogin", userName).catch(function (err) {
		return console.error(err.toString());
	});
};

function UpdateFiefdom() {
	//console.log("here");
	connection.invoke("RequestFiefdomData").catch(function (err) {
		return console.error(err.toString());
	});
};

function BuyResource(type, quantity) {
	//console.log("bought");
	connection.invoke("BuyResource", type, quantity).catch(function (err) {
		return console.error(err.toString());
	});
};

function SellResource(type, quantity) {
	connection.invoke("SellResource", type, quantity).catch(function (err) {
		return console.error(err.toString());
	});
};

connection.on("ReceiveMarketPrices", function (prices) {
	prices.forEach(function (x) {
		market[x.type] = x.price;
	});
});

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



connection.on("RecieveFiefdomData", function (plots, gameState, gameValues) {
	//console.log(gameValues);
	if (plots == null) {
		window.location.href = "/";
	}
	//Plots
	for (var i = 0; i < plots.fiefdomPlot.length; i++) {
		fief.plots[i] = plots.fiefdomPlot[i].type;
	}
	//Resources
	plots.fiefdomResources.forEach(function (p) {
		fief.resources[p.type] = p.quantity;
	});
	fief.title = plots.title;

	fief.gameState = gameState;
	fief.tax = gameValues.marketTax;

	plots.fiefdomResources.forEach(function (p) {
		fief.resources[p.type] = p.quantity;
	});

	//gameState ballots
	fief.ballots = [];
	if (gameValues.ballots != null) {
		gameValues.ballots.forEach(function (p) {
			var format = '';
			psplit = p.split(' ');
			if (psplit[0] == "Tax") {
				format = "Market Tax of " + psplit[1] + "%\n";
			} else if (psplit[0] == "Levy") {
				format = psplit[0] + " " + psplit[2] + "%  of " + psplit[1] + "\n";
			} else {
				format = "Reduce value of " + psplit[1] + " by " + psplit[2] + " %\n";
			}
			fief.ballots.push(format);
		});
	} else {
		fief.ballots.push("Empty");
		fief.ballots.push("Empty");
		fief.ballots.push("Empty");
	}
	//gameState edicts
	fief.edicts = [];
	if (gameValues.edicts != null) {
		gameValues.edicts.forEach(function (p) {
			fief.edicts.push(p);
		});
	} else {
		fief.edicts.push({ type: "Empty", Amount: 0 });
		fief.edicts.push({ type: "Empty", Amount: 0 });
		fief.edicts.push({ type: "Empty", Amount: 0 });
	}
	
	//gameState Market
	fief.baseMarket = [];
	fief.buyMarket = [];
	fief.sellMarket = [];

	gameValues.baseMarket.forEach(function (p) {
		fief.baseMarket.push(p);
	});

	gameValues.buyMarket.forEach(function (p) {
		fief.buyMarket.push(p);
	});

	gameValues.sellMarket.forEach(function (p) {
		fief.sellMarket.push(p);
	});

	fief.vote[0] = plots.ballot1;
	fief.vote[1] = plots.ballot2;
	fief.vote[2] = plots.ballot3;

	if (initialStart === false) {
		game.scene.run('scene');
		initialStart = true;
	}
});
