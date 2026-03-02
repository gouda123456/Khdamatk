using Stripe;
using Stripe.Checkout;

namespace Khdamatk.Server.Helper;

public class PaymentHelper(
    IOptions<StripeSetting> options,
    CustomerService customerService,
    //TokensService tokensService,
    PaymentIntentService paymentIntentService,
    ProductService productService,
    SubscriptionService subscriptionService,
    PriceService priceService,
    InvoiceService invoiceService,
    ChargeService chargeService,
    RefundService refundService
    //SessionService sessionService
    ) : IPaymentHelper
{



    private readonly StripeSetting StripeSetting = options.Value;
    private readonly CustomerService customerService = customerService;
   // private readonly TokensService tokensService = tokensService;
    private readonly PaymentIntentService paymentIntentService = paymentIntentService;
    private readonly ProductService productService = productService;
    private readonly SubscriptionService subscriptionService = subscriptionService;
    private readonly PriceService priceService = priceService;
    private readonly InvoiceService invoiceService = invoiceService;
    private readonly ChargeService chargeService = chargeService;
    private readonly RefundService refundService = refundService;
    //private readonly SessionService sessionService = sessionService;

    public async Task<bool> Pay(string PriceId, string CustomerId)
    {

        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string>
            {
                "card",
            },
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    Price = PriceId,
                    Quantity = 1,
                },
            },
            Mode = "payment",
            SuccessUrl = "https://example.com/success",
            CancelUrl = "https://example.com/cancel",
            Customer = CustomerId
        };


        SessionService sessionService = new SessionService();
        Session session = await sessionService.CreateAsync(options);

        return false;
    }

    //TODO: Put the method in AuthService and make it create a customer with the same email as the user when they register,
    //then return the CustomerId to be stored in the database for later use in payments
    public async Task<Customer?> AddCustomer(string CustomerId, string UserName, string Email)
    {
        var Customeroptions = new CustomerCreateOptions
        {
            Name = UserName,
            Email = Email,
            
        };


        return await customerService.CreateAsync(Customeroptions);
    }

    public async Task<Customer?> GetCustomerByEmail(string Email)
    {
        var options = new CustomerListOptions
        {
            Email = Email,
            Limit = 1
        };
        var customers = await customerService.ListAsync(options);
        return customers.Data.FirstOrDefault();
    }

    public async Task<Product?> AddProduct(string Name, string Description, long Price, string Currency)
    {
        var productOptions = new ProductCreateOptions
        {
            Name = Name,
            Description = Description,
        };
        Product product = await productService.CreateAsync(productOptions);
        var priceOptions = new PriceCreateOptions
        {
            UnitAmount = Price,
            Currency = Currency,
            Product = product.Id,
        };
        await priceService.CreateAsync(priceOptions);
        return product;
    }

    public async Task<StripeList<Product>?> GetAllProducts()
    {
        var options = new ProductListOptions()
        {
           Expand = new List<string> { "data.default_price" }
        };

        StripeList<Product>? products = await productService.ListAsync(options);

        return products;
    }

    public async Task<Product?> GetProductById(string ProductId)
    {
        var options = new ProductGetOptions()
        {
            Expand = new List<string> { "default_price" }
        };
        return await productService.GetAsync(ProductId, options);
    }

    public async Task<Price?> GetPriceById(string PriceId)
    {
        return await priceService.GetAsync(PriceId);
    }

    public async Task<Session> CreateCheckoutSession(string PriceId, string CustomerId, string SuccessUrl, string CancelUrl)
    {
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string>
            {
                "card",
            },
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    Price = PriceId,
                    Quantity = 1,
                },
            },
            Mode = "payment",
            SuccessUrl = SuccessUrl,
            CancelUrl = CancelUrl,
            Customer = CustomerId
        };




        SessionService sessionService = new SessionService();
        Session session = await sessionService.CreateAsync(options);

        return session;
    }


    // استبدلنا PriceId بـ Amount و Currency واسم الخدمة، وأضفنا المعرفات للـ Metadata
    public async Task<string> Pay (
        decimal amount,
        string currency,
        string serviceName,
        string orderId,
        string transactionId,
        string customerId,
        string successUrl,
        string cancelUrl)
    {
        var options = new SessionCreateOptions
        {
            Customer = customerId,
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<SessionLineItemOptions>
        {
            new SessionLineItemOptions
            {
                // استخدام PriceData لإنشاء السعر مباشرة دون الحاجة لـ Product Id مسبق
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(amount * 100), // تحويل للقرش/سنت
                    Currency = currency.ToLower(),
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = serviceName
                    }
                },
                Quantity = 1,
            },
        },
            Mode = "payment",
            SuccessUrl = successUrl,
            CancelUrl = cancelUrl,
            // الأهم: ربط العملية بقاعدة بيانات Khdamatk
            Metadata = new Dictionary<string, string>
        {
            { "OrderId", orderId },
            { "TransactionId", transactionId }
        }
        };

        // افترض أننا قمنا بحقن sessionService في الـ Constructor
        SessionService sessionService = new SessionService();
        Session session = await sessionService.CreateAsync(options);

        // نُرجع الرابط فقط (أو DTO) لكي لا نلوث طبقة الـ Application بـ Stripe Session
        return session.Url;
    }
}
