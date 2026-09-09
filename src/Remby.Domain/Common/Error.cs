namespace ECommerce.Domain.Common;

public record Error(string Message, ErrorType ErrorType)
{
    public static Error None => new Error(string.Empty, ErrorType.None);
    
    private static Error Validation(string message = "Invalid data format") => new(message, ErrorType.Validation);
    private static Error NotFound(string message = "Resource not found") => new(message, ErrorType.NotFound);
    private static Error BusinessRule(string message = "Broken business rule") => new(message, ErrorType.BusinessRule);  
    private static Error Conflict(string message = "Conflict data") => new(message, ErrorType.Conflict);  
    private static Error Internal(string message = "Something went wrong") => new(message, ErrorType.Internal);
    private static Error Unavailable(string massage = "Service Unavailable") => new(massage, ErrorType.Unavailable);
    private static Error Timeout(string massage = "Gateway Timeout") => new(massage, ErrorType.Timeout);
    
    public static class ChemicalElement
    {
        public static Error InvalidAtomicNumber => Validation($"Atomic number must be between 1 and 118");
        public static Error InvalidSymbol => Validation($"Symbol must be 1-3 characters");
        public static Error EmptyName => Validation("Element name cannot be empty");
        public static Error NegativeAtomicMass => Validation($"Atomic mass must be positive");
        public static Error NotFound => NotFound($"Chemical Element not found");
    }
    
    public static class Category
    {
        public static Error EmptyName => Validation("Category name cannot be empty");
        public static Error NameTooLong(int maxLength) => 
            Validation($"Category name cannot exceed {maxLength} characters");
        public static Error DescriptionTooLong(int maxLength) => 
            Validation($"Description cannot exceed {maxLength} characters");
    }
    
    public static class Money
    {
        public static Error NegativeAmount(decimal amount) => Validation($"Amount cannot be negative. Got: {amount}");
        public static Error InvalidCurrency(string currency) => Validation($"Currency {currency} is invalid");
        public static Error TooManyDecimalPlaces => Validation("Can't be lenght greater than two");
    }
    
    public static class Database
    {
        public static Error ConnectionFailed => Unavailable("Connection to server failed");
        public static Error TimeoutGateway => Timeout("Timeout connection to server");
        public static Error InternalServer => Internal("Internal Server Error");
    }
}

public enum ErrorType
{
    None = 000,
    Validation = 400,
    NotFound = 	404,
    BusinessRule = 422,
    Conflict = 409,
    Unavailable = 503,
    Internal = 500,
    Timeout = 504
}