using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos.Fulfillment;

    public class Order
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("admin_graphql_api_id")]
        public string AdminGraphqlApiId { get; set; }

        [JsonPropertyName("app_id")]
        public object AppId { get; set; }

        [JsonPropertyName("browser_ip")]
        public string BrowserIp { get; set; }

        [JsonPropertyName("buyer_accepts_marketing")]
        public bool BuyerAcceptsMarketing { get; set; }

        [JsonPropertyName("cancel_reason")]
        public string CancelReason { get; set; }

        [JsonPropertyName("cancelled_at")]
        public object CancelledAt { get; set; }

        [JsonPropertyName("cart_token")]
        public string CartToken { get; set; }

        [JsonPropertyName("checkout_id")]
        public long CheckoutId { get; set; }

        [JsonPropertyName("checkout_token")]
        public string CheckoutToken { get; set; }

        [JsonPropertyName("client_details")]
        public ClientDetails ClientDetails { get; set; }

        [JsonPropertyName("closed_at")]
        public object ClosedAt { get; set; }

        [JsonPropertyName("confirmation_number")]
        public object ConfirmationNumber { get; set; }

        [JsonPropertyName("confirmed")]
        public bool Confirmed { get; set; }

        [JsonPropertyName("contact_email")]
        public string ContactEmail { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("current_subtotal_price")]
        public string CurrentSubtotalPrice { get; set; }

        [JsonPropertyName("current_subtotal_price_set")]
        public AmountSet CurrentSubtotalPriceSet { get; set; }

        [JsonPropertyName("current_total_additional_fees_set")]
        public AmountSet CurrentTotalAdditionalFeesSet { get; set; }

        [JsonPropertyName("current_total_discounts")]
        public string CurrentTotalDiscounts { get; set; }

        [JsonPropertyName("current_total_discounts_set")]
        public AmountSet CurrentTotalDiscountsSet { get; set; }

        [JsonPropertyName("current_total_duties_set")]
        public AmountSet CurrentTotalDutiesSet { get; set; }

        [JsonPropertyName("current_total_price")]
        public string CurrentTotalPrice { get; set; }

        [JsonPropertyName("current_total_price_set")]
        public AmountSet CurrentTotalPriceSet { get; set; }

        [JsonPropertyName("current_total_tax")]
        public string CurrentTotalTax { get; set; }

        [JsonPropertyName("current_total_tax_set")]
        public AmountSet CurrentTotalTaxSet { get; set; }

        [JsonPropertyName("customer_locale")]
        public object CustomerLocale { get; set; }

        [JsonPropertyName("device_id")]
        public object DeviceId { get; set; }

        [JsonPropertyName("discount_codes")]
        public List<DiscountCode> DiscountCodes { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("estimated_taxes")]
        public bool EstimatedTaxes { get; set; }

        [JsonPropertyName("financial_status")]
        public string FinancialStatus { get; set; }

        [JsonPropertyName("fulfillment_status")]
        public object FulfillmentStatus { get; set; }

        [JsonPropertyName("landing_site")]
        public string LandingSite { get; set; }

        [JsonPropertyName("landing_site_ref")]
        public string LandingSiteRef { get; set; }

        [JsonPropertyName("location_id")]
        public object LocationId { get; set; }

        [JsonPropertyName("merchant_of_record_app_id")]
        public object MerchantOfRecordAppId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("note")]
        public string Note { get; set; }

        [JsonPropertyName("note_attributes")]
        public List<NoteAttribute> NoteAttributes { get; set; }

        [JsonPropertyName("number")]
        public int Number { get; set; }

        [JsonPropertyName("order_number")]
        public int OrderNumber { get; set; }

        [JsonPropertyName("order_status_url")]
        public string OrderStatusUrl { get; set; }

        [JsonPropertyName("original_total_additional_fees_set")]
        public AmountSet OriginalTotalAdditionalFeesSet { get; set; }

        [JsonPropertyName("original_total_duties_set")]
        public AmountSet OriginalTotalDutiesSet { get; set; }

        [JsonPropertyName("payment_gateway_names")]
        public List<string> PaymentGatewayNames { get; set; }

        [JsonPropertyName("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("po_number")]
        public string PoNumber { get; set; }

        [JsonPropertyName("presentment_currency")]
        public string PresentmentCurrency { get; set; }

        [JsonPropertyName("processed_at")]
        public DateTime ProcessedAt { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("referring_site")]
        public string ReferringSite { get; set; }

        [JsonPropertyName("source_identifier")]
        public string SourceIdentifier { get; set; }

        [JsonPropertyName("source_name")]
        public string SourceName { get; set; }

        [JsonPropertyName("source_url")]
        public object SourceUrl { get; set; }

        [JsonPropertyName("subtotal_price")]
        public string SubtotalPrice { get; set; }

        [JsonPropertyName("subtotal_price_set")]
        public AmountSet SubtotalPriceSet { get; set; }

        [JsonPropertyName("tags")]
        public string Tags { get; set; }

        [JsonPropertyName("tax_exempt")]
        public bool TaxExempt { get; set; }

        [JsonPropertyName("tax_lines")]
        public List<TaxLine> TaxLines { get; set; }

        [JsonPropertyName("taxes_included")]
        public bool TaxesIncluded { get; set; }

        [JsonPropertyName("test")]
        public bool Test { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("total_discounts")]
        public string TotalDiscounts { get; set; }

        [JsonPropertyName("total_discounts_set")]
        public AmountSet TotalDiscountsSet { get; set; }

        [JsonPropertyName("total_line_items_price")]
        public string TotalLineItemsPrice { get; set; }

        [JsonPropertyName("total_line_items_price_set")]
        public AmountSet TotalLineItemsPriceSet { get; set; }

        [JsonPropertyName("total_outstanding")]
        public string TotalOutstanding { get; set; }

        [JsonPropertyName("total_price")]
        public string TotalPrice { get; set; }

        [JsonPropertyName("total_price_set")]
        public AmountSet TotalPriceSet { get; set; }

        [JsonPropertyName("total_shipping_price_set")]
        public AmountSet TotalShippingPriceSet { get; set; }

        [JsonPropertyName("total_tax")]
        public string TotalTax { get; set; }

        [JsonPropertyName("total_tax_set")]
        public AmountSet TotalTaxSet { get; set; }

        [JsonPropertyName("total_tip_received")]
        public string TotalTipReceived { get; set; }

        [JsonPropertyName("total_weight")]
        public int TotalWeight { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("user_id")]
        public object UserId { get; set; }

        [JsonPropertyName("billing_address")]
        public BillingAddress BillingAddress { get; set; }

        [JsonPropertyName("customer")]
        public Customer Customer { get; set; }

        [JsonPropertyName("discount_applications")]
        public List<DiscountApplication> DiscountApplications { get; set; }

        [JsonPropertyName("fulfillments")]
        public List<Fulfillment> Fulfillments { get; set; }

        [JsonPropertyName("line_items")]
        public List<LineItem> LineItems { get; set; }

        [JsonPropertyName("payment_terms")]
        public object PaymentTerms { get; set; }

        [JsonPropertyName("refunds")]
        public List<Refund> Refunds { get; set; }

        [JsonPropertyName("shipping_address")]
        public ShippingAddress ShippingAddress { get; set; }

        [JsonPropertyName("shipping_lines")]
        public List<ShippingLine> ShippingLines { get; set; }
    }