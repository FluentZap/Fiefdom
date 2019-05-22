var connection = new signalR.HubConnectionBuilder().withUrl("/fiefdomHub").build();


var fief = {};

fief.plots = [];
fief.resources = {};
fief.gameState = {};
fief.market = {};
fief.ballots = [];
fief.tax = 0;
fief.edicts = [{type: "Empty", Amount: 0},{type: "Empty", Amount: 0},{type: "Empty", Amount: 0}];


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

connection.on("RecieveFiefdomData", function (plots, gameState, market, gameValues) {
	
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

	fief.ballots = [];
	if(gameValues.ballots != null){
		gameValues.ballots.forEach(function (p) {
			var format = '';
		psplit = p.split(' ');
		if(psplit[0] == "Tax"){
			format = "Market Tax of " + psplit[1] + "%\n";
		}else if (psplit[0] == "Levy") {
			format = psplit[0] + " " + psplit[2] + "%  of " + psplit[1] + "\n";
		}else{
			format = "Reduce value of " + psplit[1] + " by " + psplit[2] + " %\n";
		}
		fief.ballots.push(format);
	});
}else{
	fief.ballots.push("Empty");
	fief.ballots.push("Empty");
	fief.ballots.push("Empty");
}
fief.tax = gameValues.markettax;

fief.edicts = [];
	if(gameValues.edicts != null){
		gameValues.edicts.forEach(function (p) {
			console.log(p);
		fief.edicts.push(p);
	});
}else{
	fief.edicts.push({type: "Empty", Amount: 0});
	fief.edicts.push({type: "Empty", Amount: 0});
	fief.edicts.push({type: "Empty", Amount: 0});
}

	if (initialStart === false)
	{
		game.scene.run('scene');
		initialStart = true;
	}
});
