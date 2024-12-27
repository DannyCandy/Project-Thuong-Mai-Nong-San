class OrderHistory {
  String? orderId;
  String? userId;
  int? totalPrice;
  String? orderTime;
  String? message;
  String? dcGiaoHang;
  bool? success;
  String? paymentMethod;
  String? phone;
  String? idVoucher;
  String? idCtkm;
  List<OrderDetails>? orderDetails;

  OrderHistory(
      {this.orderId,
        this.userId,
        this.totalPrice,
        this.orderTime,
        this.message,
        this.dcGiaoHang,
        this.success,
        this.paymentMethod,
        this.phone,
        this.idVoucher,
        this.idCtkm,
        this.orderDetails});

  OrderHistory.fromJson(Map<String, dynamic> json) {
    orderId = json['orderId'];
    userId = json['userId'];
    totalPrice = (json['totalPrice'] as num?)?.toInt();
    orderTime = json['orderTime'];
    message = json['message'];
    dcGiaoHang = json['dcGiaoHang'];
    success = json['success'];
    paymentMethod = json['paymentMethod'];
    phone = json['phone'];
    idVoucher = json['idVoucher'];
    idCtkm = json['idCtkm'];
    if (json['orderDetails'] != null) {
      orderDetails = <OrderDetails>[];
      json['orderDetails'].forEach((v) {
        orderDetails!.add(new OrderDetails.fromJson(v));
      });
    }
  }

  String extractDate() {
    DateTime dateTime = DateTime.parse(this.orderTime!);

    int year = dateTime.year;
    int month = dateTime.month;
    int day = dateTime.day;

    return '$day-$month-$year';
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['orderId'] = this.orderId;
    data['userId'] = this.userId;
    data['totalPrice'] = this.totalPrice;
    data['orderTime'] = this.orderTime;
    data['message'] = this.message;
    data['dcGiaoHang'] = this.dcGiaoHang;
    data['success'] = this.success;
    data['paymentMethod'] = this.paymentMethod;
    data['phone'] = this.phone;
    data['idVoucher'] = this.idVoucher;
    data['idCtkm'] = this.idCtkm;
    if (this.orderDetails != null) {
      data['orderDetails'] = this.orderDetails!.map((v) => v.toJson()).toList();
    }
    return data;
  }
}

class OrderDetails {
  String? orderDetailId;
  int? quantity;
  String? idsp;
  int? price;

  OrderDetails({this.orderDetailId, this.quantity, this.idsp, this.price});

  OrderDetails.fromJson(Map<String, dynamic> json) {
    orderDetailId = json['orderDetailId'];
    quantity = json['quantity'];
    idsp = json['idsp'];
    price = (json['price'] as num?)?.toInt();
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['orderDetailId'] = this.orderDetailId;
    data['quantity'] = this.quantity;
    data['idsp'] = this.idsp;
    data['price'] = this.price;
    return data;
  }
}
