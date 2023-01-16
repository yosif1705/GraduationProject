using Server;
using System.Data;

class DbActions
{
    private readonly Database _database;

    public DbActions(Database database)
    {
        _database = database;
    }

    public async Task<bool> Login(string identifier, string password)
    {

        string query = "SELECT COUNT(*) FROM users WHERE (username = @identifier OR email = @identifier) AND password = @password";
        var parameters = new Dictionary<string, object>
        {
            { "@identifier", identifier },
            { "@password", password }
        };

        var result = await _database.ExecuteScalar(query, parameters);

        return Convert.ToInt32(result) > 0;
    }



    public async Task<bool> Register(string username, string email, string password)
    {
        string query = "INSERT INTO users (username, email, password) VALUES (@username, @email, @password)";
        var parameters = new Dictionary<string, object>
        {
            { "@username", username },
            { "@email", email },
            { "@password", password }
        };

        var result = await _database.ExecuteNonQuery(query, parameters);

        return result > 0;
    }

    public async Task<DataTable> GetAllSongs()
    {
        string query = "SELECT * FROM songs";

        return await _database.ExecuteDataTable(query);
    }
    public async Task<bool> AddSongToPlaylist(int playlistId, int songId, int playlistIndex)
    {
        string query = "INSERT INTO playlist_songs (playlist_id, song_id,playlist_index) VALUES (@playlistId, @songId, @playlistIndex)";
        var parameters = new Dictionary<string, object>
        {
            { "@playlistId", playlistId },
            { "@songId", songId },
            { "@playlistIndex",playlistIndex }
        };

        var result = await _database.ExecuteNonQuery(query, parameters);

        return result > 0;
    }
    public async Task<DataTable> GetUsersPlaylists(int userId)
    {
        string query = "SELECT * FROM playlists WHERE creator_id = @userId";
        var parameters = new Dictionary<string, object>
        {
            { "@userId", userId }
        };

        return await _database.ExecuteDataTable(query, parameters);
    }
    public async Task<bool> UpdateUserStatus(int userId, string status)
    {
        string query = "UPDATE users SET status = @status WHERE user_id = @userId";
        var parameters = new Dictionary<string, object>
        {
            { "@userId", userId },
            { "@status", status }
        };

        var result = await _database.ExecuteNonQuery(query, parameters);

        return result > 0;
    }
    public async Task<bool> UpdateUserPlayingSong(int userId, int songId)
    {
        string query = "UPDATE users SET playing_song_id = @songId WHERE user_id = @userId";
        var parameters = new Dictionary<string, object>
        {
            { "@userId", userId },
            { "@songId", songId }
        };

        var result = await _database.ExecuteNonQuery(query, parameters);

        return result > 0;
    }
    public async Task<DataTable> GetSongsInPlaylist(int playlistId)
    {
        string query = "SELECT song_id FROM playlist_songs WHERE playlist_id = @playlistId ORDER BY playlist_index";
        var parameters = new Dictionary<string, object>
        {
            { "@playlistId", playlistId }
        };

        return await _database.ExecuteDataTable(query, parameters);
    }

    public async Task<bool> RemoveSongFromPlaylist(int playlistId, int songId)
    {
        string query = "DELETE FROM playlist_songs WHERE playlist_id = @playlistId and song_id = @songId";
        var parameters = new Dictionary<string, object>
        {
            { "@playlistId", playlistId },
            { "@songId", songId }
        };

        var result = await _database.ExecuteNonQuery(query, parameters);

        return result > 0;
    }
    public async Task<bool> RenamePlaylist(int playlistId, string newName)
    {
        string query = "UPDATE playlists SET name = @newName WHERE playlist_id = @playlistId";
        var parameters = new Dictionary<string, object>
        {
            { "@playlistId", playlistId },
            { "@newName", newName }
        };

        var result = await _database.ExecuteNonQuery(query, parameters);

        return result > 0;
    }
}