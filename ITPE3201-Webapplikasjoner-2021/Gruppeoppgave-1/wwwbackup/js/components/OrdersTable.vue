<template>
  <v-data-table
        :headers='headers'
        :items='orders'
    />
</template>

<script>
module.exports = {
    name: 'OrdersTable',
    data() {
        return {
            orders: [{}],
            headers: [
                {
                    text: 'Order ID',
                    value: 'id',
                },
                {
                    text: 'Customer',
                    value: 'customer.email'
                },
                {
                    text: 'Origin',
                    value: 'departureRouteTime.route.origin.name'
                },
                {
                    text: 'Departure',
                    value: 'departure'
                },
                {
                    text: 'Destination',
                    value: 'departureRouteTime.route.destination.name'
                },
                {
                    text: 'Return',
                    value: 'return'
                },
                {
                    text: 'Tur-Retur',
                    value: 'turRetur'
                },
                {
                    text: 'Passengers',
                    value: 'passengers'
                },
                {
                    text: 'Vehicles',
                    value: 'vehicles'
                },
                {
                    text: 'Total price',
                    value: 'totalPrice'
                }
            ],
        }
    },
    mounted() {
        fetch('/orders')
            .then(response => response.json())
            .then(orders => {
                this.orders = orders.map(order => {
                    return {
                        departure: new Date(order.departureRouteTime.date).toLocaleString('nb-NO', { dateStyle: 'short' }),
                        return: order.returnRouteTime != null ? new Date(order.returnRouteTime.date).toLocaleString('nb-NO', { dateStyle: 'short' }) : null,
                        totalPrice: (
                            (
                                order.departureRouteTime.price +
                                (order.returnRouteTime != null ? order.returnRouteTime?.price : 0)
                            ) * (order.passengers + order.vehicles * 3)
                            ).toLocaleString('nb-NO', 
                                {
                                    style: 'currency',
                                    currency: 'NOK',
                                    maximumFractionDigits: 0
                                }
                            ) + ",-",
                        turRetur: order.isRoundTrip ? "✓" : "𐄂",
                        ...order
                    }
                })
            })
            .catch(error => {
                alert('Noe gikk galt: ' + error)
            }) 
    }
}
</script>