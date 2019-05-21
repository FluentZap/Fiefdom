var sprite;

let ratio = 720 / 216;

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


	this.marketBackground = this.add.tileSprite(0, 0, 821, 507, 'marketBorder').setOrigin(0, 0).setScrollFactor(0);
};

function createPlayer() {

	//Add Character
	this.player = this.physics.add.sprite(0, 0, 'character');
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

function buildPlots(){
	var plotGroup = this.physics.add.staticGroup();
	this.plotGroup = plotGroup;
	var x = 0;
	var y = 710;
	var imgKey = "log";
	this.plots = [];

	for (var i = 0; i < fief.plots.length; i++) {
		x += 300;
		this.plots[i] = plotGroup.create(x, y, imgKey);
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
		UpdateFiefdom();
		console.log(fief.plots);
		console.log(fief.resources);
	}

	if (cursors.left.isDown) {
		player.setVelocityX(-200); // move left
		moving = true;
		player.flipX = true;
	}
	else if (cursors.right.isDown) {
		player.setVelocityX(200);
		moving = true;
		player.flipX = false;
	} else {
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
	this.gold.setText("Gold " + fief.resources.Gold);
}



//keydown objects/events
function initKeys(){
	this.downKey = this.input.keyboard.addKey('DOWN');
}

function downIsDown(){
		if(Phaser.Input.Keyboard.JustDown(this.downKey)){
		return true;
	}
	return false;
}

function plotMenuDisplay(player, plot){
	console.log(plot);
}






class Fiefdom extends Phaser.Scene {

preload() {
	//Background Images
	this.load.image('mill', 'assets/mill.png');
	this.load.image('log', 'assets/logPile.png');
	this.load.image('bg', 'assets/BG.png');
	this.load.image('bg1', 'assets/plx-1.png');
	this.load.image('bg2', 'assets/plx-2.png');
	this.load.image('bg3', 'assets/plx-3.png');
	this.load.image('bg4', 'assets/plx-4.png');
	this.load.image('bg5', 'assets/plx-5.png');
	this.load.image('marketBorder', 'assets/marketWindow.png');

	//Sprite Sheets


	this.load.spritesheet('character', 'assets/adventurer-Sheet.png', { frameWidth: 50, frameHeight: 37 });
};

//Create
create() {
	createBackgrounds.call(this);
 	buildPlots.call(this);
	initKeys.call(this);


	var platforms = this.physics.add.staticGroup();
	platforms.create(16 * 2, game.config.height - 16 * 2, 'bg').setScale(4).refreshBody();
	//groundLayer = map.createDynamicLayer('World', groundTiles, 0, 0);

	//this.bg.setScrollFactor(0);


	createPlayer.call(this);
	createPlayerAnimation.call(this);


	this.physics.add.collider(this.player, platforms);
	this.physics.add.overlap(this.player, this.plotGroup, plotMenuDisplay, downIsDown, this);


	this.physics.world.bounds.width = 4000;
	//this.physics.world.bounds.height = 800;
	this.cameras.main.setBounds(0, 0, 4000, 720);
	this.cameras.main.startFollow(this.player);

	this.cursors = this.input.keyboard.createCursorKeys();
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


	this.gold = this.add.text(40, 40, "Fiefdom", {
		//alagard.ttf
		font: "40px Alagard",
		fill: "#ff0044",
		align: "center"
	});
	this.gold.setScrollFactor(0);
	//this.gold.anchor.setTo(0.5, 0.5);

};

update(time, theta) {
	updateBackground.call(this);
	updatePlayerUi.call(this);
	updateUi.call(this);
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
			debug: true
		}
	}
};

var game = new Phaser.Game(config);
var newScene = game.scene.add('scene', Fiefdom, false);
