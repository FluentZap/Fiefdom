var sprite;

var goldTotal = 0;
var woodTotal = 0;
var stoneTotal = 0;
var metalTotal = 0;
var foodTotal = 0;

let ratio = 720 / 216;
var GameScene = new Phaser.Class({

	Extends: Phaser.Scene,

	initialize:


	function GameScene() {
		Phaser.Scene.call(this, { key: 'gameScene', active: true });

		this.player = null;
		this.cursors = null;
		this.player = null;
		this.cursors = null;
		this.goldCount = null;
		this.woodCount = null;
		this.stoneCount = null;
		this.metalCount = null;
		this.foodCount = null;
	},


	preload: function () {
		this.load.image('bg', 'assets/BG.png');
		this.load.image('bg1', 'assets/plx-1.png');
		this.load.image('bg2', 'assets/plx-2.png');
		this.load.image('bg3', 'assets/plx-3.png');
		this.load.image('bg4', 'assets/plx-4.png');
		this.load.image('bg5', 'assets/plx-5.png');
		this.load.spritesheet('character', 'assets/adventurer-Sheet.png', { frameWidth: 50, frameHeight: 37 });


	},

	//Create
	create: function () {
		this.bg1 = this.add.tileSprite(0, 0, game.config.width, game.config.height, 'bg1').setOrigin(0, 0);
		this.bg2 = this.add.tileSprite(0, 0, game.config.width, game.config.height, 'bg2').setOrigin(0, 0);
		this.bg3 = this.add.tileSprite(0, 0, game.config.width, game.config.height, 'bg3').setOrigin(0, 0);
		this.bg4 = this.add.tileSprite(0, 0, game.config.width, game.config.height, 'bg4').setOrigin(0, 0);
		this.bg5 = this.add.tileSprite(0, 0, game.config.width, game.config.height, 'bg5').setOrigin(0, 0);
		//this.bg = this.add.tileSprite(0, game.config.height - 16, game.config.width, 16, 'bg').setOrigin(0, 0);

		goldCount = this.add.text(50, 20, '💰: 0', {fontSize: '20px'}).setScrollFactor(0);
		woodCount = this.add.text(50, 50, '🌳: 0', {fontSize: '20px'}).setScrollFactor(0);
		stoneCount = this.add.text(50, 80, '🎸: 0', {fontSize: '20px'}).setScrollFactor(0);
		metalCount = this.add.text(50, 110, '🗡: 0', {fontSize: '20px'}).setScrollFactor(0);
		foodCount = this.add.text(50, 140, '🍖: 0', {fontSize: '20px'}).setScrollFactor(0);

		var platforms = this.physics.add.staticGroup();
		platforms.create(16 * 2, game.config.height - 16 * 2, 'bg').setScale(4).refreshBody();
		//groundLayer = map.createDynamicLayer('World', groundTiles, 0, 0);

		// TEMPERARY
		var plots = this.physics.add.staticGroup();
		this.plot1 = plots.create(500, 710, 'bg');
		this.plot2 = plots.create(900, 710, 'bg');
		this.plots = plots;

		//this.bg.setScrollFactor(0);
		this.bg1.setScrollFactor(0);
		this.bg2.setScrollFactor(0);
		this.bg3.setScrollFactor(0);
		this.bg4.setScrollFactor(0);
		this.bg5.setScrollFactor(0);

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


		this.player = this.physics.add.sprite(0, 0, 'character');
		this.player.setBounce(0.2);
		this.player.setCollideWorldBounds(true);
		this.player.setScale(3);
		//this.player.body.setSize(14, 7, 31, 35);
		this.player.body.setSize(25, 32, false);
		this.player.body.setOffset(14, 4);

		this.physics.add.collider(this.player, platforms);

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

		// TEMPERARY
		this.bKey = this.input.keyboard.addKey('b');
		this.physics.add.overlap(this.player, plots, this.plotBuild, this.bIsDown, this);
	},

	update: function (time, theta) {
		var cursors = this.cursors;
		var player = this.player;
		this.bg1.tilePositionX = this.cameras.main.scrollX * .2 / ratio;
		this.bg2.tilePositionX = this.cameras.main.scrollX * .4 / ratio;
		this.bg3.tilePositionX = this.cameras.main.scrollX * .6 / ratio;
		this.bg4.tilePositionX = this.cameras.main.scrollX * .8 / ratio;
		this.bg5.tilePositionX = this.cameras.main.scrollX * 1 / ratio;

		let onGround = (player.body.touching.down || player.body.blocked.down);
		let moving = false;
		if (cursors.up.isDown && onGround) {
			player.setVelocityY(-330);
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


	},

	bIsDown: function(){
		if(this.bKey.isDown){
			return true;
		}
		return false;
	},

	plotBuild: function(player, plot){
		countResources()
	}

});

function countResources()
{
	this.goldTotal += 10;
	goldCount.setText('💰: ' + goldTotal);
	this.woodTotal += 5;
	woodCount.setText('🌳: ' + woodTotal);
	this.stoneTotal += 3;
	stoneCount.setText('🎸: ' + stoneTotal);
	this.metalTotal += 8;
	metalCount.setText('🗡: ' + metalTotal);
	this.foodTotal += 1;
	foodCount.setText('🍖: ' + foodTotal);
}


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
	},
	scene: GameScene
};

var game = new Phaser.Game(config);
