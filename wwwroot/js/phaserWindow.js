var sprite;
var marketVisable = false;
var voteVisable = false;
let ratio = 720 / 216;
var vote = [];
vote.push("None");
vote.push("None");
vote.push("None");

//create canvas objects
function createBackgrounds() {
	//Add backgrounds
	this.bg1 = this.add.tileSprite(0, 0, game.config.width, game.config.height, 'bg1').setOrigin(0, 0).setScrollFactor(0);
	this.bg2 = this.add.tileSprite(0, 0, game.config.width, game.config.height, 'bg2').setOrigin(0, 0).setScrollFactor(0);
	this.bg3 = this.add.tileSprite(0, 0, game.config.width, game.config.height, 'bg3').setOrigin(0, 0).setScrollFactor(0);
	this.bg4 = this.add.tileSprite(0, 0, game.config.width, game.config.height, 'bg4').setOrigin(0, 0).setScrollFactor(0);
	this.bg5 = this.add.tileSprite(0, 0, game.config.width, game.config.height, 'bg5').setOrigin(0, 0).setScrollFactor(0);

	this.bg1.setDisplaySize(game.config.width, game.config.height);
	this.bg1.setScale(ratio);
	this.bg2.setDisplaySize(game.config.width, game.config.height);
	this.bg2.setScale(ratio);
	this.bg3.setDisplaySize(game.config.width, game.config.height);
	this.bg3.setScale(ratio);
	this.bg4.setDisplaySize(game.config.width, game.config.height);
	this.bg4.setScale(ratio);
	this.bg5.setDisplaySize(game.config.width, game.config.height);
	this.bg5.setScale(ratio);

	//Depth guide
	//player 9999
	//top bar icons 600s
	//sidebar 600s
	//market menu 500s
	//voiting 400s
	//build menu icons 300s
	//buildings 10s

	//market
	this.marketBackground = this.add.tileSprite(game.config.width / 2, game.config.height / 2, 821, 507, 'marketBorder').setScrollFactor(0).setDepth(550).setVisible(false);
	var Id = 0
	this.marketMenu = [];

	var marketItems = ["Food", "Wood", "Stone"];

	for (y = 300; y <= 450; y += 75) {
		//320 370 910 960
		let marketBuyLarge = this.add.image(910, y, 'rightArrow').setScrollFactor(0).setDepth(600).setVisible(false);
		marketBuyLarge.Id = "Market Buy 10 " + marketItems[Id];
		marketBuyLarge.setInteractive().on('pointerdown', (item) => { handleClick.call(this, marketBuyLarge.Id) });
		this.marketMenu.push(marketBuyLarge);

		let marketBuySmall = this.add.image(960, y, 'rightArrow').setScrollFactor(0).setDepth(600).setVisible(false);
		marketBuySmall.Id = "Market Buy 1 " + marketItems[Id];
		marketBuySmall.setInteractive().on('pointerdown', (item) => { handleClick.call(this, marketBuySmall.Id) });
		this.marketMenu.push(marketBuySmall);

		let marketSellLarge = this.add.image(370, y, 'leftArrow').setScrollFactor(0).setDepth(600).setVisible(false);
		marketSellLarge.Id = "Market Sell 10 " + marketItems[Id];
		marketSellLarge.setInteractive().on('pointerdown', (item) => { handleClick.call(this, marketSellLarge.Id) });
		this.marketMenu.push(marketSellLarge);

		let marketSellSmall = this.add.image(320, y, 'leftArrow').setScrollFactor(0).setDepth(600).setVisible(false);
		marketSellSmall.Id = "Market Sell 1 " + marketItems[Id];
		marketSellSmall.setInteractive().on('pointerdown', (item) => { handleClick.call(this, marketSellSmall.Id) });
		this.marketMenu.push(marketSellSmall);

		Id++;
	}
	this.marketMenu.push(this.add.image(600, 300, 'foodIcon').setDisplaySize(64, 64).setScrollFactor(0).setDepth(600).setVisible(false));
	this.marketMenu.push(this.add.image(600, 375, 'woodIcon').setDisplaySize(64, 64).setScrollFactor(0).setDepth(600).setVisible(false));
	this.marketMenu.push(this.add.image(600, 450, 'stoneIcon').setDisplaySize(64, 64).setScrollFactor(0).setDepth(600).setVisible(false));

	this.marketMenu.push(this.add.text(320, 200, "Sell \n1   10", { font: "32px Alagard", fill: "#000000", align: "center" }).setScrollFactor(0).setDepth(600).setVisible(false));
	this.marketMenu.push(this.add.text(910, 200, "Buy \n10   1", { font: "32px Alagard", fill: "#000000", align: "center" }).setScrollFactor(0).setDepth(600).setVisible(false));


	this.marketCost = [];
	//food
	this.marketCost.push(this.add.text(640, 290, "", { font: "32px Alagard", fill: "#000000", align: "center" }).setScrollFactor(0).setDepth(600).setVisible(false));
	this.marketCost.push(this.add.text(520, 290, "", { font: "32px Alagard", fill: "#77dd77", align: "center" }).setScrollFactor(0).setDepth(600).setVisible(false));
	this.marketCost.push(this.add.text(740, 290, "", { font: "32px Alagard", fill: "#ff6961", align: "center" }).setScrollFactor(0).setDepth(600).setVisible(false));

	this.marketCost.push(this.add.text(640, 440, "", { font: "32px Alagard", fill: "#000000", align: "center" }).setScrollFactor(0).setDepth(600).setVisible(false));
	this.marketCost.push(this.add.text(520, 440, "", { font: "32px Alagard", fill: "#77dd77", align: "center" }).setScrollFactor(0).setDepth(600).setVisible(false));
	this.marketCost.push(this.add.text(740, 440, "", { font: "32px Alagard", fill: "#ff6961", align: "center" }).setScrollFactor(0).setDepth(600).setVisible(false));

	this.marketCost.push(this.add.text(640, 365, "", { font: "32px Alagard", fill: "#000000", align: "center" }).setScrollFactor(0).setDepth(600).setVisible(false));
	this.marketCost.push(this.add.text(520, 365, "", { font: "32px Alagard", fill: "#77dd77", align: "center" }).setScrollFactor(0).setDepth(600).setVisible(false));
	this.marketCost.push(this.add.text(740, 365, "", { font: "32px Alagard", fill: "#ff6961", align: "center" }).setScrollFactor(0).setDepth(600).setVisible(false));

	this.marketTitleText = this.add.text(640, 200, "", { font: "32px Alagard", fill: "#000000", align: "center" }).setScrollFactor(0).setDepth(600).setVisible(false);

	this.titleButton = this.add.image(960, 550, 'titleIcon').setScrollFactor(0).setDepth(600).setVisible(false).setDisplaySize(50,50);
	this.titleButton.setInteractive().on('pointerdown', (item) => { BuyTitle(); UpdateFiefdom.call(this) });
	this.marketCost.push(this.add.text(700, 530, "Buy New Title", { font: "32px Alagard", fill: "#000000", align: "center" }).setScrollFactor(0).setDepth(600).setVisible(false));
	this.marketMenu.push(this.titleButton);

	//ground
	this.ground = this.add.tileSprite(0, game.config.height - 20, 6000, 20, 'bg').setOrigin(0, 0);
	this.ground.setScale(ratio);

	//buildings
	this.building1 = this.add.image(1925, 470, 'building1').setDepth(100);
	this.building2 = this.add.image(500, 530, 'building2').setVisible(false).setDepth(80);
	this.building3 = this.add.image(1070, 420, 'building3').setVisible(false);
	this.building4 = this.add.image(2500, 385, 'building4').setVisible(false).setDepth(90);
	this.building5 = this.add.image(1500, 380, 'building5').setDisplaySize(400, 690).setVisible(false).setDepth(99);
	this.building6 = this.add.image(3000, 450, 'building6').setVisible(false);
	this.building7 = this.add.image(3500, 420, 'building7').setVisible(false);
	this.building8 = this.add.image(4000, 400, 'building8').setVisible(false).setDepth(70);
	this.building9 = this.add.image(4600, 440, 'building9').setVisible(false);
	this.building10 = this.add.image(4250, 380, 'building5').setDisplaySize(400, 690).setVisible(false);
	this.building11 = this.add.image(0, 420, 'building7').setVisible(false).setDepth(81);
	this.buildingGroup = [this.building1, this.building2, this.building3, this.building4, this.building5, this.building6, this.building7, this.building8, this.building9, this.building10]

	//build menu group
	this.foodIcon = this.add.image(0,0, 'foodIcon').setDisplaySize(100,100).setVisible(false).setDepth(300);
	this.foodIcon.Id = "BuildSelect Farm";
	this.woodIcon = this.add.image(0,0, 'woodIcon').setDisplaySize(100,100).setVisible(false).setDepth(301);
	this.woodIcon.Id = "BuildSelect WoodCutter";
	this.stoneIcon = this.add.image(0,0, 'stoneIcon').setDisplaySize(100,100).setVisible(false).setDepth(302);
	this.stoneIcon.Id = "BuildSelect Quarry";
	this.barracksIcon = this.add.image(0,0, 'barracksIcon').setDisplaySize(100,100).setVisible(false).setDepth(304);
	this.barracksIcon.Id = "BuildSelect Barracks";
	this.innIcon = this.add.image(0,0, 'innIcon').setDisplaySize(100,100).setVisible(false).setDepth(305);
	this.innIcon.Id = "BuildSelect Inn";
	this.goldIcon = this.add.image(0,0, 'goldIcon').setDisplaySize(100,100).setVisible(false).setDepth(306);
	this.goldIcon.Id = "BuildSelect Gold";
	this.buildMenu = [this.foodIcon, this.woodIcon, this.stoneIcon, this.barracksIcon, this.innIcon, this.goldIcon];

	//confirm menu group
	this.confirmIcon = this.add.image(0,0, 'upgradeIcon').setDisplaySize(100,100).setVisible(false).setDepth(307);
	this.confirmName = this.add.text(0, 0, 'Building Name:', { font: "32px Alagard", fill: "#000000", align: "center" }).setVisible(false).setDepth(308);
	this.confirmCost = this.add.text(0,0, ' Cost', { font: "32px Alagard", fill: "#000000", align: "center" }).setVisible(false).setDepth(309);
	this.confirmGroup = [this.confirmIcon, this.confirmName, this.confirmCost];

	//voting window
	this.voteBG = this.add.image(650, 200, 'voteBG').setScrollFactor(0).setVisible(false).setDepth(10).setDepth(400);
	this.voteText = this.add.text(440, 100, "", { font: "28px Alagard", fill: "#000000", align: "center" }).setScrollFactor(0).setVisible(false).setDepth(401);
	this.voteYes1 = this.add.image(400, 120, "thumbsUp").setDisplaySize(75, 75).setScrollFactor(0).setVisible(false).setDepth(402);
	this.voteNo1 = this.add.image(900, 120, "thumbsDown").setDisplaySize(75, 75).setScrollFactor(0).setVisible(false).setDepth(403);
	this.voteYes2 = this.add.image(400, 185, "thumbsUp").setDisplaySize(75, 75).setScrollFactor(0).setVisible(false).setDepth(405);
	this.voteNo2 = this.add.image(900, 185, "thumbsDown").setDisplaySize(75, 75).setScrollFactor(0).setVisible(false).setDepth(406);
	this.voteYes3 = this.add.image(400, 250, "thumbsUp").setDisplaySize(75, 75).setScrollFactor(0).setVisible(false).setDepth(407);
	this.voteNo3 = this.add.image(900, 250, "thumbsDown").setDisplaySize(75, 75).setScrollFactor(0).setVisible(false).setDepth(408);

	this.voteGroup = [this.voteBG, this.voteText, this.voteYes1, this.voteNo1,this.voteYes2, this.voteNo2,this.voteYes3, this.voteNo3];

	this.voteYes1.setInteractive().on('pointerdown', (item) => {
		handleClick.call(this, "Vote Fore 0");
	})

	this.voteNo1.setInteractive().on('pointerdown', (item) => {
		handleClick.call(this, "Vote Nay 0");
	})

	this.voteYes2.setInteractive().on('pointerdown', (item) => {
		handleClick.call(this, "Vote Fore 1");
	})

	this.voteNo2.setInteractive().on('pointerdown', (item) => {
		handleClick.call(this, "Vote Nay 1");
	})

	this.voteYes3.setInteractive().on('pointerdown', (item) => {
		handleClick.call(this, "Vote Fore 2");
	})

	this.voteNo3.setInteractive().on('pointerdown', (item) => {
		handleClick.call(this, "Vote Nay 2");
	})

	this.buildMenu.forEach((item) =>{
		item.setInteractive().on('pointerdown', (id) => {
			handleClick.call(this, item.Id)
		});
	})

	this.confirmIcon.setInteractive().on('pointerdown', (item) => {
		handleClick.call(this, "Build")

	});

	//resource group
	this.rbGold = this.add.image(30, 40, 'goldIcon').setDisplaySize(50,50).setScrollFactor(0).setDepth(600);
	this.gold = this.add.text(65, 20, "Fiefdom", {
		font: "40px Alagard",
		fill: "#000000",
		align: "center"
	}).setScrollFactor(0).setDepth(640);
	this.rbFood = this.add.image(200, 40, 'foodIcon').setDisplaySize(50,50).setScrollFactor(0).setDepth(601);
	this.food = this.add.text(235, 20, "Fiefdom", {
		font: "40px Alagard",
		fill: "#000000",
		align: "center"
	}).setScrollFactor(0).setDepth(641);
	this.rbWood = this.add.image(370, 40, 'woodIcon').setDisplaySize(50,50).setScrollFactor(0).setDepth(602);
	this.wood = this.add.text(405, 20, "Fiefdom", {
		font: "40px Alagard",
		fill: "#000000",
		align: "center"
	}).setScrollFactor(0).setDepth(643);
	this.rbStone = this.add.image(540, 40, 'stoneIcon').setDisplaySize(50,50).setScrollFactor(0).setDepth(603);
	this.stone = this.add.text(575, 20, "Fiefdom", {
		font: "40px Alagard",
		fill: "#000000",
		align: "center"
	}).setScrollFactor(0).setDepth(644);

	//game time group
	this.date = this.add.text(800, 20, "Fiefdom", { font: "40px Alagard", fill: "#000000", align: "center" }).setScrollFactor(0).setDepth(604);



	this.ballots = this.add.text(40, 190, "Fiefdom", {
		font: "40px Alagard",
		fill: "#000000",
		align: "center"
	}).setScrollFactor(0).setVisible(false);
	this.edicts = this.add.text(40, 310, "Fiefdom", {
		font: "40px Alagard",
		fill: "#000000",
		align: "center"
	}).setScrollFactor(0);
	this.edicts.setVisible(false);

	//sidebar
	this.sbTaxIcon = this.add.image(30, 150, "coinIcon").setDisplaySize(50,50).setScrollFactor(0).setDepth(620);
	this.sbFoodIcon = this.add.image(30, 225, "foodIcon").setDisplaySize(50,50).setScrollFactor(0).setDepth(621);
	this.sbWoodIcon = this.add.image(30, 300, "woodIcon").setDisplaySize(50,50).setScrollFactor(0).setDepth(622);
	this.sbStoneIcon = this.add.image(30, 375, "stoneIcon").setDisplaySize(50,50).setScrollFactor(0).setDepth(623);

	this.sbTaxText = this.add.text(65, 125, "", {
		font: "40px Alagard",
		fill: "#000000",
		align: "center"
	}).setScrollFactor(0).setDepth(624);
	this.sbFoodText = this.add.text(65, 200,  "", {
		font: "40px Alagard",
		fill: "#000000",
		align: "center"
	}).setScrollFactor(0).setDepth(624);
	this.sbWoodText = this.add.text(65, 275,  "", {
		font: "40px Alagard",
		fill: "#000000",
		align: "center"
	}).setScrollFactor(0).setDepth(624);
	this.sbStoneText = this.add.text(65, 350,  "", {
		font: "40px Alagard",
		fill: "#000000",
		align: "center"
	}).setScrollFactor(0).setDepth(624);

};

function handleClick(id) {
	var type = id.split(' ');

	if (type[0] == "BuildSelect") {
		setVisible(this.confirmGroup, true);
		this.buildItem = type[1];
		this.confirmName.setText(type[1]);
		this.confirmCost.setText(100);
	}

	if (type[0] == "Build") {
		//console.log(this.selectedPlot, this.buildItem);
		BuildPlot(this.selectedPlot, this.buildItem);
		setVisible(this.confirmGroup, false);
		setVisible(this.buildMenu, false);
		UpdateFiefdom.call(this);
		if (!this.hammer.isPlaying) this.hammer.play();
	}

	if (type[0] == "Vote") {

		if (fief.vote[type[2]] == type[1]) {
			fief.vote[type[2]] = "vote";
			SubmitVote(type[2], "vote");
		} else {
			fief.vote[type[2]] = type[1];
			SubmitVote(type[2], type[1]);
			if (!this.boo.isPlaying) this.boo.play();

		}
		if (!this.rabble.isPlaying) this.rabble.play();

	}

	if (type[0] == "Market") {
		if (type[1] == "Buy") {
			BuyResource(type[3], type[2]);
		}

		if (type[1] == "Sell") {
			SellResource(type[3], type[2]);
		}
		UpdateFiefdom();
	}


	// Selling sound effect
	//	this.coins.play();
	//	or this.chaching.play();

	// BuildPlot(this.selectedPlot, id);
	// setVisible(this.confirmGroup, false);

	// displayConfirmMenu(this.selectedPlot, item.id);
	// // BuildPlot(this.selectedPlot, item.Id)
	// console.log("Done");
	// // setVisible(this.buildMenu, false);
	// UpdateFiefdom();
}

function build(id) {
	//console.log(id);
}

function toggleVote() {
	voteVisable = !voteVisable;
	setVisible(this.voteGroup, voteVisable);

	if (!this.rabble.isPlaying) this.rabble.play();
	if (!this.order.isPlaying) this.order.play();
	
	if (voteVisable)
	{
		this.marketVisable = false;
		setVisible(this.marketMenu, false);
		setVisible(this.marketCost, false);
		this.marketBackground.setVisible(false);
		this.marketTitleText.setVisible(false);
	}
	
	//setTimeout(function () { this.order.play(); }, 5000);
	// setVisible(this.voteGroup, true);	
}

function createPlayer() {

	//Add Character
	this.player = this.physics.add.sprite(1925, game.config.height, 'character').setDepth(9999);
	this.player.setBounce(0.2);
	this.player.setCollideWorldBounds(true);
	this.player.setScale(3);
	//this.player.body.setSize(14, 7, 31, 35);
	this.player.body.setSize(25, 32, false);
	this.player.body.setOffset(14, 4);
}

function createPlayerAnimation() {
	this.anims.create({
		key: 'walk',
		frames: this.anims.generateFrameNumbers('character', { start: 8, end: 13 }),
		frameRate: 6,
		repeat: -1
	});

	this.anims.create({
		key: 'idle',
		frames: this.anims.generateFrameNumbers('character', { start: 0, end: 3 }),
		frameRate: 2,
		repeat: -1
	});

	this.anims.create({
		key: 'jump',
		frames: this.anims.generateFrameNumbers('character', { start: 16, end: 23 }),
		frameRate: 8,
		repeat: 0
	});
};

function buildPlots() {
	var plotGroup = this.physics.add.staticGroup();
	this.plotGroup = plotGroup;
	this.plotGroup.sfx = {}
	var x = 0;
	var y = 690;
	var imgKey = "lockIcon";
	this.plots = [];

	for (var i = 0; i < fief.plots.length; i++) {
		x += 350;
		if (x == 1750) {
			x = 2450;
		}
		this.plots[i] = plotGroup.create(x, y, imgKey).setDisplaySize(75,75).setDepth(500);
		this.plots[i].Id = i;
	}
}

// update functions
function updateBackground() {
	this.bg1.tilePositionX = this.cameras.main.scrollX * .2 / ratio;
	this.bg2.tilePositionX = this.cameras.main.scrollX * .4 / ratio;
	this.bg3.tilePositionX = this.cameras.main.scrollX * .6 / ratio;
	this.bg4.tilePositionX = this.cameras.main.scrollX * .8 / ratio;
	this.bg5.tilePositionX = this.cameras.main.scrollX * 1 / ratio;
}

function updatePlayerUi() {
	var cursors = this.cursors;
	var player = this.player;

	let onGround = (player.body.touching.down || player.body.blocked.down);
	let moving = false;
	if (cursors.up.isDown && onGround) {
		player.setVelocityY(-330);
		if (!this.grunt.isPlaying) this.grunt.play();
		//BuildPlot(6,'Farm')
		//UpdateFiefdom();
		//console.log(fief.plots);
		//console.log(fief.resources);
		//console.log("Title is " + fief.title);
	}

	if (cursors.left.isDown) {
		player.setVelocityX(-400); // move left
		moving = true;
		player.flipX = true;
	}

	else if (cursors.right.isDown) {
		player.setVelocityX(400);
		moving = true;
		player.flipX = false;
	}

	else {
		player.setVelocityX(0);
	}

	if (!onGround) {
		player.anims.play('jump', true);
	} else if (moving) {
		player.anims.play('walk', true);
	} else {
		player.anims.play('idle', true);
	}
}

function updateUi() {
	this.gold.setText(fief.resources.Gold);
	this.food.setText(fief.resources.Food);
	this.wood.setText(fief.resources.Wood);
	this.stone.setText(fief.resources.Stone);
	this.date.setText("Day " + fief.gameState.day + "  Season " + fief.gameState.season + "  Year " + fief.gameState.year);
	var influence = parseInt(fief.resources.Gold / 1000 + (1 + fief.title) * 10);

	this.voteText.setText("Influence: " + influence + "\n" + fief.ballots[0] + "\n" + fief.ballots[1] + "\n" + fief.ballots[2])
	//parse Text
	// this.ballots.setText(fief.ballots[0] + fief.ballots[1] + fief.ballots[2]);
	if (fief.edicts.length === 3)
	{
		var foodDiscount = 0;
		var stoneDiscount = 0;
		var woodDiscount = 0;
		var taxTotal = 0;
		for(i=0; i < fief.edicts.length; i++){
			if(fief.edicts[i].type == "Tax"){
				taxTotal = taxTotal + parseInt(fief.edicts[i].amount);
			}
			if(fief.edicts[i].type == "Market"){
				switch(fief.edicts[i].target){
					case "Food": foodDiscount += parseInt(fief.edicts[i].amount);
						break;
					case "Wood": woodDiscount += parseInt(fief.edicts[i].amount);
						break;
					case "Stone": stoneDiscount += parseInt(fief.edicts[i].amount);
						break;
				}
			}
			this.sbTaxText.setText("+" + taxTotal + "%");
			this.sbFoodText.setText("-" + foodDiscount + "%");
			this.sbWoodText.setText("-" + woodDiscount + "%");
			this.sbStoneText.setText("-" + stoneDiscount + "%");
		}
	}
	if (this.seasonSound != fief.gameState.season) {
		if (!this.frog.isPlaying) this.frog.play();
		this.seasonSound = fief.gameState.season
	}
	//console.log(fief);
	for (i = 0; i < fief.baseMarket.length; i++) {
		this.marketCost[i * 3 + 1].setText(fief.sellMarket[i].price + "\n" + fief.sellMarket[i].price * 10);
		this.marketCost[i * 3 + 2].setText(fief.buyMarket[i].price + "\n" + fief.buyMarket[i].price * 10);
		this.marketCost[i * 3].setText(fief.baseMarket[i].price + "\n" + fief.baseMarket[i].price * 10);
	}

	if (fief.vote[0] == "Fore") { this.voteYes1.setTint(0x00ff00); this.voteNo1.setTint(0xff0000); }
	if (fief.vote[0] == "Nay") { this.voteYes1.setTint(0xff0000); this.voteNo1.setTint(0x00ff00); }
	if (fief.vote[0] == "vote") { this.voteYes1.setTint(0xffffff); this.voteNo1.setTint(0xffffff); }

	if (fief.vote[1] == "Fore") { this.voteYes2.setTint(0x00ff00); this.voteNo2.setTint(0xff0000); }
	if (fief.vote[1] == "Nay") { this.voteYes2.setTint(0xff0000); this.voteNo2.setTint(0x00ff00); }
	if (fief.vote[1] == "vote") { this.voteYes2.setTint(0xffffff); this.voteNo2.setTint(0xffffff); }

	if (fief.vote[2] == "Fore") { this.voteYes3.setTint(0x00ff00); this.voteNo3.setTint(0xff0000); }
	if (fief.vote[2] == "Nay") { this.voteYes3.setTint(0xff0000); this.voteNo3.setTint(0x00ff00); }
	if (fief.vote[2] == "vote") { this.voteYes3.setTint(0xffffff); this.voteNo3.setTint(0xffffff); }
	switch(fief.title)
	{
		case 0:
		this.marketTitleText.setText("Yeoman")
		break;
		case 1:
		this.marketTitleText.setText("Patrician")
		break;
		case 2:
		this.marketTitleText.setText("Baron")
		break;
		case 3:
		this.marketTitleText.setText("Lord")
		break;
	}

}

//keydown objects/events
function initKeys() {
	this.downKey = this.input.keyboard.addKey('DOWN');
	this.mKey = this.input.keyboard.addKey("M");
	this.vKey = this.input.keyboard.addKey("V");
	this.qKey = this.input.keyboard.addKey("Q");
	this.yKey = this.input.keyboard.addKey("Y");
	// Pause music
	this.pKey = this.input.keyboard.addKey("P");
}

function downIsDown() {
	if (Phaser.Input.Keyboard.JustDown(this.downKey)) {
		return true;
	}
	return false;
}

function plotMenuDisplay(player, plot) {
	this.foodIcon.x = plot.x - 120;
	this.foodIcon.y = plot.y - 150;

	this.woodIcon.x = plot.x;
	this.woodIcon.y = plot.y - 150;

	this.stoneIcon.x = plot.x + 120;
	this.stoneIcon.y = plot.y - 150;

	this.barracksIcon.x = plot.x - 120;
	this.barracksIcon.y = plot.y - 270;

	this.innIcon.x = plot.x;
	this.innIcon.y = plot.y - 270;

	this.goldIcon.x = plot.x + 120;
	this.goldIcon.y = plot.y - 270;

	buildConfirmMenu(plot, this.confirmGroup);

	this.selectedPlot = plot.Id;

	if (!this.anvil.isPlaying) this.anvil.play();

	//console.log(fief.plots[plot.Id]);
	if (fief.plots[plot.Id] != "Locked") {
		setVisible(this.buildMenu, true);
	}
}

function buildConfirmMenu(plot, confirmGroup) {

	confirmGroup[1].x = plot.x - 120;
	confirmGroup[1].y = plot.y - 400;

	confirmGroup[2].x = plot.x;
	confirmGroup[2].y = plot.y - 400;

	confirmGroup[0].x = plot.x + 180;
	confirmGroup[0].y = plot.y - 400;
}

function toggleMarket() {
	marketVisable = !marketVisable;
	setVisible(this.marketMenu, marketVisable);
	setVisible(this.marketCost, marketVisable);	
	this.marketBackground.setVisible(marketVisable);
	this.marketTitleText.setVisible(marketVisable);
	if (marketVisable)
	{	
		voteVisable = false;
		setVisible(this.voteGroup, false);
		setVisible(this.voteGroup, false);
	}
	if (!this.chaching.isPlaying) this.chaching.play();
}


function setVisible(array, value) {
	array.forEach(function (item) {
		item.setVisible(value);
	});
}

function updatePlots() {
	// console.log(this.plots);
	for(i=0; i<fief.plots.length; i++)
	{
		if(fief.plots[i] != "Empty" && fief.plots[i] != "Locked") {
			this.plots[i].setVisible(false);
			this.buildingGroup[i].setVisible(true);
		}

		if(fief.plots[i] == "Empty") {
			this.plots[i].setTexture('unlocked');
			// this.plots[i].setVisible(false);
			// this.buildingGroup[i].setVisible(true);
		}
		
		// switch(fief.plots[i]){
		// 	case "Empty": this.plots[i].setTexture('log').refreshBody();
		// 	break;
		// 	case "Farm": {
		// 		this.plots[i].setVisible(false);
		// 		this.building2.setVisible(true);
		// 		UpdateFiefdom.call(this);
		// 	}
		// 	break;
		// 	case "Locked": {
		// 		this.plots[i].setTexture('lockIcon').refreshBody();
		// 		this.plots[i].y = 680;
		// 		this.plots[i].setDisplaySize(75,75);
		// 	}
		// 	break;
		// }
	}
}


class Fiefdom extends Phaser.Scene {

	preload() {
		//icons
		this.load.image('woodIcon', 'assets/icons/woodIcon.png');
		this.load.image('stoneIcon', 'assets/icons/stoneIcon.png');
		this.load.image('foodIcon', 'assets/icons/foodIcon.png');
		this.load.image('coinIcon', 'assets/icons/coinIcon.png');
		this.load.image('anvilIcon', 'assets/icons/anvilIcon.png');
		this.load.image('goldIcon', 'assets/icons/goldIcon.png');
		this.load.image('influenceIcon', 'assets/icons/influenceIcon.png');
		this.load.image('innIcon', 'assets/icons/innIcon.png');
		this.load.image('ironIcon', 'assets/icons/ironIcon.png');
		this.load.image('lockIcon', 'assets/icons/lockIcon.png');
		this.load.image('marketIcon', 'assets/icons/marketIcon.png');
		this.load.image('titleIcon', 'assets/icons/titleIcon.png');
		this.load.image('upgradeIcon', 'assets/icons/upgradeIcon.png');
		this.load.image('voteIcon', 'assets/icons/voteIcon.png');
		this.load.image('barracksIcon', 'assets/icons/barracksIcon.png');
		this.load.image('thumbsUp', 'assets/thumbsup.png');
		this.load.image('thumbsDown', 'assets/thumbsdown.png');
		this.load.image('unlocked', 'assets/icons/unlockedIcon.png');

		//buildings
		this.load.image('building1', 'assets/buildings/building1.png');
		this.load.image('building2', 'assets/buildings/building2.png');
		this.load.image('building3', 'assets/buildings/building3.png');
		this.load.image('building4', 'assets/buildings/building4.png');
		this.load.image('building5', 'assets/buildings/building5.png');
		this.load.image('building6', 'assets/buildings/building6.png');
		this.load.image('building7', 'assets/buildings/building7.png');
		this.load.image('building8', 'assets/buildings/building8.png');
		this.load.image('building9', 'assets/buildings/building9.png');


		//menu backgrounds
		this.load.image('marketBorder', 'assets/marketWindow.png');
		this.load.image('voteBG', 'assets/scroll2.png');

		//Background Images

		this.load.image('leftArrow', 'assets/leftArrow.png');
		this.load.image('rightArrow', 'assets/rightArrow.png');
		this.load.image('bg', 'assets/BG.png');
		this.load.image('bg1', 'assets/plx-1.png');
		this.load.image('bg2', 'assets/plx-2.png');
		this.load.image('bg3', 'assets/plx-3.png');
		this.load.image('bg4', 'assets/plx-4.png');
		this.load.image('bg5', 'assets/plx-5.png');

		//Sprite Sheets
		this.load.spritesheet('character', 'assets/adventurer-Sheet.png', { frameWidth: 50, frameHeight: 37 });


		// Music
		this.load.audio('synth', 'assets/audio/synth.mp3');
		//
		// // Sounds
		this.load.audio('anvil', 'assets/audio/anvil.mp3');
		this.load.audio('boo', 'assets/audio/boo.mp3');
		this.load.audio('hammer', 'assets/audio/build.mp3');
		this.load.audio('chaching', 'assets/audio/chaching.mp3');
		this.load.audio('cheers', 'assets/audio/cheers.mp3');
		this.load.audio('coins', 'assets/audio/coins.mp3');
		this.load.audio('frog', 'assets/audio/frog.mp3');
		this.load.audio('frog2', 'assets/audio/frog2.mp3');
		this.load.audio('grunt', 'assets/audio/grunt.mp3');
		this.load.audio('step', 'assets/audio/step.mp3');
		this.load.audio('synth', 'assets/audio/synth.mp3');
		this.load.audio('rabble', 'assets/audio/rabble.mp3');
		this.load.audio('order', 'assets/audio/order.mp3');
		this.load.audio('toot', 'assets/audio/toot.mp3');

		//preloader
		var progressBar = this.add.graphics();
		var progressBox = this.add.graphics();
		progressBox.fillStyle(0x222222, 0.8);
		progressBox.fillRect(320, 330, 600, 50);

		var width = this.cameras.main.width;
		var height = this.cameras.main.height;
		var loadingText = this.make.text({
			x: width / 2,
			y: height / 2 - 50,
			text: 'Loading...',
			style: {
				font: '20px monospace',
				fill: '#ffffff'
			}
		});
		loadingText.setOrigin(0.5, 0.5);
		var percentText = this.make.text({
			x: width / 2,
			y: height / 2 - 5,
			text: '0%',
			style: {
				font: '18px monospace',
				fill: '#ffffff'
			}
		});
		percentText.setOrigin(0.5, 0.5);
		var assetText = this.make.text({
			x: width / 2,
			y: height / 2 + 50,
			text: '',
			style: {
				font: '18px monospace',
				fill: '#ffffff'
			}
		});
		assetText.setOrigin(0.5, 0.5);

		this.load.on('progress', function(value){
			console.log(value);
			percentText.setText(parseInt(value * 100) + '%');

			progressBar.clear();
			progressBar.fillStyle(0xff0000, 1);
			progressBar.fillRect(340, 340, 560 * value, 30);
		})
		this.load.on('fileprogress', function (file) {
			assetText.setText('Loading asset: ' + file.key);
		});
		this.load.on('complete', function () {
			console.log('complete');
			progressBar.destroy();
			progressBox.destroy();
			loadingText.destroy();
			percentText.destroy();
			assetText.destroy();
		});
	};


	//Create
	create() {


		createBackgrounds.call(this);
		buildPlots.call(this);
		initKeys.call(this);

		//keypresses
		// this.mKey.addListener(, toggleMarket);

		this.seasonSound = fief.gameState.season;

		createPlayer.call(this);
		createPlayerAnimation.call(this);

		//overlaps
		this.physics.add.overlap(this.player, this.plotGroup, plotMenuDisplay, downIsDown, this);

		//gamebounds and camera
		this.physics.world.bounds.width = 6000;
		this.physics.world.bounds.height = 710;
		this.cameras.main.setBounds(0, 0, 6000, 720);
		this.cameras.main.startFollow(this.player);

		this.cursors = this.input.keyboard.createCursorKeys();

		//sound fx
		this.music = this.sound.add('synth', {volume: 0.25});
		this.anvil = this.sound.add('anvil');
		this.boo = this.sound.add('boo');
		this.hammer = this.sound.add('hammer');
		this.chaching = this.sound.add('chaching');
		this.coins = this.sound.add('coins');
		this.frog = this.sound.add('frog');
		this.frog2 = this.sound.add('frog2');
		this.grunt = this.sound.add('grunt', {volume: 0.75});
		this.step = this.sound.add('step');
		this.rabble = this.sound.add('rabble', { volume: 1.2 });
		this.order = this.sound.add('order');
		this.toot = this.sound.add('toot', { volume: 1.5 });
		this.cheers = this.sound.add('cheers');

		this.music.loop = true;
		this.music.play();


		//fullscreen keypress
		var FKey = this.input.keyboard.addKey('F');

		FKey.on('down', function () {

			if (this.scale.isFullscreen) {
				//button.setFrame(0);
				this.scale.stopFullscreen();
			}
			else {
				//button.setFrame(1);
				this.scale.startFullscreen();
			}
		}, this);

	};


	update(time, theta) {
		updateBackground.call(this);
		updatePlayerUi.call(this);
		updateUi.call(this);
		if (Phaser.Input.Keyboard.JustDown(this.mKey)) {
			toggleMarket.call(this);
		}
		if (Phaser.Input.Keyboard.JustDown(this.vKey)) {
			toggleVote.call(this);
		}
		if (Phaser.Input.Keyboard.JustDown(this.qKey)) {
			if (!this.toot.isPlaying) this.toot.play();
		}
		if(Phaser.Input.Keyboard.JustDown(this.yKey)){
			this.frog2.play();
		}

		// Mute music
		if(Phaser.Input.Keyboard.JustDown(this.pKey)){
			// this.music.isPaused = !this.music.isPaused;
			if (this.music.isPaused === false){
				this.music.pause();
			} else {
				this.music.resume();
			}
		}

		if (this.player.x > this.woodIcon.x + 150 || this.player.x < this.woodIcon.x - 150) {
			setVisible(this.buildMenu, false);
		}
		updatePlots.call(this);
	};

};

var config = {
	type: Phaser.AUTO,
	scale: {
		mode: Phaser.Scale.FIT,
		parent: 'fiefdom-body',
		autoCenter: Phaser.Scale.CENTER_BOTH,
		width: 1280,
		height: 720
	},
	pixelArt: true,
	antialias: false,
	physics: {
		default: 'arcade',
		arcade: {
			gravity: { y: 500 },
			debug: false
		}
	}
};

var game = new Phaser.Game(config);
var newScene = game.scene.add('scene', Fiefdom, false);
