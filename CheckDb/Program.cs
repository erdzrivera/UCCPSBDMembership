using System;
using Npgsql;

var connString = "Host=dpg-d9e2fsv7f7vs739n2a4g-a.singapore-postgres.render.com;Database=membership_g9sf;Username=membership_g9sf_user;Password=MzVJTpwSKkbN9mCoZ6hJYOvRFN2sLVeo;Ssl Mode=Require;Trust Server Certificate=true;";

using var conn = new NpgsqlConnection(connString);
conn.Open();

using var cmd2 = new NpgsqlCommand("SELECT count(*) FROM \"OpenIddictApplications\"", conn);
Console.WriteLine("Apps: " + cmd2.ExecuteScalar());

using var cmd3 = new NpgsqlCommand("SELECT \"ClientId\", \"RedirectUris\" FROM \"OpenIddictApplications\"", conn);
using var reader = cmd3.ExecuteReader();
while (reader.Read())
{
    Console.WriteLine(reader.GetString(0) + " -> " + reader.GetString(1));
}
