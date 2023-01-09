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
    playing_song_id INTEGER DEFAULT NULL
);

CREATE TABLE songs (
    song_id INTEGER PRIMARY KEY AUTO_INCREMENT,
    
    uploader_id INTEGER NOT NULL,
    title VARCHAR(255) NOT NULL,
    duration INTEGER NOT NULL,
    `path` VARCHAR(255) NOT NULL,
    is_local BOOLEAN NOT NULL
);

CREATE TABLE playlists (
    playlist_id INTEGER PRIMARY KEY AUTO_INCREMENT,
    
    creator_id INTEGER NOT NULL,
    `name` VARCHAR(255) NOT NULL
);

CREATE TABLE playlist_songs (
    playlist_id INTEGER NOT NULL,
    song_id INTEGER NOT NULL,
    playlist_index INTEGER NOT NULL,
    
    PRIMARY KEY (playlist_id, song_id)
);

CREATE TABLE users_relations (
    user_id_1 INTEGER NOT NULL,
    user_id_2 INTEGER NOT NULL,
    friendship_status ENUM('REQUESTED', 'ACCEPTED', 'BLOCKED') NOT NULL,
    
    PRIMARY KEY (user_id_1, user_id_2)
);

CREATE TABLE songs_discoveries (
    user_id INTEGER NOT NULL,
    song_id INTEGER NOT NULL,
    discovery_date DATE NOT NULL,

	PRIMARY KEY (user_id, song_id)
);

ALTER TABLE users ADD FOREIGN KEY(playing_song_id) REFERENCES songs (song_id) ON DELETE SET NULL;

ALTER TABLE songs ADD FOREIGN KEY(uploader_id) REFERENCES users (user_id) ON DELETE CASCADE;

ALTER TABLE playlists ADD FOREIGN KEY (creator_id) REFERENCES users (user_id) ON DELETE CASCADE;

ALTER TABLE playlist_songs ADD FOREIGN KEY (playlist_id) REFERENCES playlists (playlist_id) ON DELETE CASCADE, 
			   ADD FOREIGN KEY (song_id) REFERENCES songs (song_id) ON DELETE CASCADE;

ALTER TABLE users_relations ADD FOREIGN KEY (user_id_1) REFERENCES users (user_id) ON DELETE CASCADE,
			    ADD FOREIGN KEY (user_id_2) REFERENCES users (user_id) ON DELETE CASCADE;

ALTER TABLE songs_discoveries ADD FOREIGN KEY (user_id) REFERENCES users (user_id) ON DELETE CASCADE,
			      ADD FOREIGN KEY (song_id) REFERENCES songs (song_id) ON DELETE CASCADE;
