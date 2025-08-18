namespace McWuT.Common.CrimeGenerator.ExternalApis;

// RandomUser API Response Models
public class RandomUserResponse
{
    public List<RandomUserResult> Results { get; set; } = new();
    public RandomUserInfo Info { get; set; } = new();
}

public class RandomUserResult
{
    public string Gender { get; set; } = string.Empty;
    public RandomUserName Name { get; set; } = new();
    public RandomUserLocation Location { get; set; } = new();
    public string Email { get; set; } = string.Empty;
    public RandomUserLogin Login { get; set; } = new();
    public RandomUserDob Dob { get; set; } = new();
    public RandomUserId Id { get; set; } = new();
    public RandomUserPicture Picture { get; set; } = new();
    public string Nat { get; set; } = string.Empty;
}

public class RandomUserName
{
    public string Title { get; set; } = string.Empty;
    public string First { get; set; } = string.Empty;
    public string Last { get; set; } = string.Empty;
}

public class RandomUserLocation
{
    public RandomUserStreet Street { get; set; } = new();
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Postcode { get; set; } = string.Empty;
    public RandomUserCoordinates Coordinates { get; set; } = new();
    public RandomUserTimezone Timezone { get; set; } = new();
}

public class RandomUserStreet
{
    public int Number { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class RandomUserCoordinates
{
    public string Latitude { get; set; } = string.Empty;
    public string Longitude { get; set; } = string.Empty;
}

public class RandomUserTimezone
{
    public string Offset { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class RandomUserLogin
{
    public string Uuid { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Salt { get; set; } = string.Empty;
    public string Md5 { get; set; } = string.Empty;
    public string Sha1 { get; set; } = string.Empty;
    public string Sha256 { get; set; } = string.Empty;
}

public class RandomUserDob
{
    public DateTime Date { get; set; }
    public int Age { get; set; }
}

public class RandomUserId
{
    public string Name { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

public class RandomUserPicture
{
    public string Large { get; set; } = string.Empty;
    public string Medium { get; set; } = string.Empty;
    public string Thumbnail { get; set; } = string.Empty;
}

public class RandomUserInfo
{
    public string Seed { get; set; } = string.Empty;
    public int Results { get; set; }
    public int Page { get; set; }
    public string Version { get; set; } = string.Empty;
}