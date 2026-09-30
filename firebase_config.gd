# firebase_config.gd
extends Node

func _ready() -> void:
	# -------------------------------------------------------------
	# 1. Firebase基本設定
	# (Firebase Consoleの「アプリ登録(Web)」でコピーした値を貼り付け)
	# -------------------------------------------------------------
	var config := {
		"apiKey": "AIzaSyByDsDCwVg1lucpqHvOBZSNG0axemeL47s",
		"authDomain": "feiptrainer.firebaseapp.com",
		"projectId": "feiptrainer",
		"storageBucket": "feiptrainer.firebasestorage.app",
		"messagingSenderId": "431530766200",
		"appId": "1:431530766200:web:0b1ae41eb9c6e25c67e28d"
	}
	
	# Firebaseプラグインに設定をセット
	Firebase.setup_config(config)
	
	# -------------------------------------------------------------
	# 2. Googleログイン（OAuth）専用設定
	# (Google Cloud Consoleで作成した「ウェブアプリケーション」のクライアント情報)
	# -------------------------------------------------------------
	# Googleプロバイダの認証情報を設定
	var google_provider = Firebase.Auth.get_auth_provider("google")
	if google_provider:
		google_provider.client_id = ""
		google_provider.client_secret = ""
		# リダイレクトポート（Google Cloud Consoleで http://localhost:5250/ を設定した場合）
		google_provider.redirect_port = 5250
