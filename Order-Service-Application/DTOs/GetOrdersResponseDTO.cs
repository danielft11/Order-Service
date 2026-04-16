namespace Order_Service_Application.DTOs
{
    public sealed record GetOrdersResponseDTO(IEnumerable<GetOrderDTO> orders);
}
