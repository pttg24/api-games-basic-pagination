namespace BasicGaming.Api.Models;

public class PaginationAInputDTO
{
    public int Offset { get; set; }
    public int Limit { get; set; }

}

public class PaginationBInputDTO
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string OrderBy { get; set; }
}