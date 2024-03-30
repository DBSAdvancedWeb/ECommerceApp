$(document).ready(function () {
    
    $('#submitOrder').click(function (e) {
        e.preventDefault();
        // create an order array
        let orderData = [];
        // Iterate through each selected product
        $('.product-item').each(function () {
            var productId = $(this).find('.productId').val();
            var quantity = $(this).find('.quantity').val();
            orderData.push({ 
                productId: parseInt(productId), 
                quantity: parseInt(quantity) 
            });
        });

        $.ajax({
            type: 'POST',
            url: '/Order/Create',
            contentType: 'application/json',
            data: JSON.stringify(orderData),
            success: function (response) {
                window.location.href = '/Order/Confirmation';
            },
            error: function (xhr, status, error) {
                // Handle error
            }
        });
    });

});