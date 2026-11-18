using System.Text.Json.Serialization;

namespace LAB04.Bai07;

public sealed class UserInfo
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("first_name")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("last_name")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("phone")]
    public string Phone { get; set; } = string.Empty;

    [JsonPropertyName("birthday")]
    public string Birthday { get; set; } = string.Empty;

    [JsonPropertyName("language")]
    public string Language { get; set; } = "vi";

    [JsonPropertyName("sex")]
    public int Sex { get; set; }
}

public sealed class SignUpRequest
{
    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("first_name")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("last_name")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("phone")]
    public string Phone { get; set; } = string.Empty;

    [JsonPropertyName("birthday")]
    public string Birthday { get; set; } = string.Empty;

    [JsonPropertyName("language")]
    public string Language { get; set; } = "vi";

    [JsonPropertyName("sex")]
    public int Sex { get; set; }
}

public sealed class TokenResponse
{
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = "Bearer";

    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;
}

public sealed class DishItem
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("ten_mon_an")]
    public string TenMonAn { get; set; } = string.Empty;

    [JsonPropertyName("gia")]
    public decimal Gia { get; set; }

    [JsonPropertyName("dia_chi")]
    public string DiaChi { get; set; } = string.Empty;

    [JsonPropertyName("hinh_anh")]
    public string HinhAnh { get; set; } = string.Empty;

    [JsonPropertyName("mo_ta")]
    public string MoTa { get; set; } = string.Empty;

    [JsonPropertyName("nguoi_dong_gop")]
    public string NguoiDongGop { get; set; } = string.Empty;
}

public sealed class CreateDishRequest
{
    [JsonPropertyName("ten_mon_an")]
    public string TenMonAn { get; set; } = string.Empty;

    [JsonPropertyName("gia")]
    public decimal Gia { get; set; }

    [JsonPropertyName("dia_chi")]
    public string DiaChi { get; set; } = string.Empty;

    [JsonPropertyName("hinh_anh")]
    public string HinhAnh { get; set; } = string.Empty;

    [JsonPropertyName("mo_ta")]
    public string MoTa { get; set; } = string.Empty;
}

public sealed class PaginationRequest
{
    [JsonPropertyName("current")]
    public int Current { get; set; } = 1;

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; } = 5;
}

public sealed class PaginationMeta
{
    [JsonPropertyName("current")]
    public int Current { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }
}

public sealed class DishPageResponse
{
    [JsonPropertyName("data")]
    public List<DishItem> Data { get; set; } = [];

    [JsonPropertyName("pagination")]
    public PaginationMeta Pagination { get; set; } = new();
}
