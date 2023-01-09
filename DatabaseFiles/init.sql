DROP SCHEMA music_streaming_app;
CREATE DATABASE music_streaming_app;
USE music_streaming_app;

CREATE TABLE users (
    user_id INTEGER PRIMARY KEY AUTO_INCREMENT,
    
    username VARCHAR(255) NOT NULL UNIQUE,
    email VARCHAR(255) NOT NULL UNIQUE,
    password_hash TEXT NOT NULL,
    `name` varchar(255),
    `status` ENUM('ONLINE', 'OFFLINE', "LISTENING") NOT NULL DEFAULT 'OFFLINE',
    playing_song_id INTEGER DEFAULT NULL,
    
	FOREIGN KEY(playing_song_id) REFERENCES songs (song_id) ON DELETE SET NULL
);

CREATE TABLE songs (
    song_id INTEGER PRIMARY KEY AUTO_INCREMENT,
    
    uploader_id INTEGER NOT NULL,
    title VARCHAR(255) NOT NULL,
    duration INTEGER NOT NULL,
    `path` VARCHAR(255) NOT NULL,
    is_local BOOLEAN NOT NULL,
    
    FOREIGN KEY(uploader_id) REFERENCES users (user_id) ON DELETE CASCADE
);

CREATE TABLE playlists (
    playlist_id INTEGER PRIMARY KEY AUTO_INCREMENT,
    
    user_id INTEGER NOT NULL,
    `name` VARCHAR(255) NOT NULL,
    
    FOREIGN KEY (user_id) REFERENCES users (user_id) ON DELETE CASCADE
);

CREATE TABLE playlist_songs (
    playlist_id INTEGER NOT NULL,
    song_id INTEGER NOT NULL,
    
    playlist_index INTEGER NOT NULL,
    
    PRIMARY KEY (playlist_id, song_id),
    FOREIGN KEY (playlist_id) REFERENCES playlists (playlist_id) ON DELETE CASCADE,
    FOREIGN KEY (song_id) REFERENCES songs (song_id) ON DELETE CASCADE
);

CREATE TABLE users_relations (
    user_id_1 INTEGER NOT NULL,
    user_id_2 INTEGER NOT NULL,
    
    friendship_status ENUM('REQUESTED', 'ACCEPTED', 'BLOCKED') NOT NULL,
    
    PRIMARY KEY (user_id_1, user_id_2, friendship_status),
    FOREIGN KEY (user_id_1) REFERENCES users (user_id) ON DELETE CASCADE,
    FOREIGN KEY (user_id_2) REFERENCES users (user_id) ON DELETE CASCADE
);

CREATE TABLE songs_discoveries (
    user_id INTEGER NOT NULL,
    song_id INTEGER NOT NULL,
    
    discovery_date DATE NOT NULL,

	PRIMARY KEY (user_id, song_id, discovery_date),	
    FOREIGN KEY (user_id) REFERENCES users (user_id) ON DELETE CASCADE,
    FOREIGN KEY (song_id) REFERENCES songs (song_id) ON DELETE CASCADE
);
