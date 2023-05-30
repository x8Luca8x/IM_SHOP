namespace IM_API
{
    public class TLANG
    {
        public string INVALID_LOGIN_DATA { get; set; } = "Invalid login data";
        public string INACTIVE_USER { get; set; } = "Inactive user";
        public string NOT_VERIFIED_USER { get; set; } = "User is not verified";
        public string UNKNOWN_ERROR { get; set; } = "Unknown error";
        public string PASSWORDS_NOT_MATCH { get; set; } = "Passwords do not match";
        public string USERNAME_ALREADY_EXISTS { get; set; } = "Username already exists";
        public string EMAIL_ALREADY_EXISTS { get; set; } = "Email already exists";
        public string INVALID_USER { get; set; } = "Invalid user";
        public string INVALID_ARTICLE_QUANTITY { get; set; } = "Article quantity must be greather then 0";
        public string CART_NOT_FOUND { get; set; } = "The current user does not have a cart";
        public string CART_ARTICLE_NOT_FOUND { get; set; } = "The current user does not have this article in the cart";
    }
}
