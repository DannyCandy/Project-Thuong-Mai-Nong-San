class OrderRequestModel {
  Cart? cart;
  String? message;

  OrderRequestModel({this.cart, this.message});

  OrderRequestModel.fromJson(Map<String, dynamic> json) {
    cart = json['cart'] != null ? new Cart.fromJson(json['cart']) : null;
    message = json['message'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    if (this.cart != null) {
      data['cart'] = this.cart!.toJson();
    }
    data['message'] = this.message;
    return data;
  }
}

class Cart {
  List<Items>? items;

  Cart({this.items});

  Cart.fromJson(Map<String, dynamic> json) {
    if (json['items'] != null) {
      items = <Items>[];
      json['items'].forEach((v) {
        items!.add(new Items.fromJson(v));
      });
    }
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    if (this.items != null) {
      data['items'] = this.items!.map((v) => v.toJson()).toList();
    }
    return data;
  }
}

class Items {
  String? productImg;
  String? productId;
  int? price;
  int? quantity;
  String? name;

  Items(
      {this.productImg, this.productId, this.price, this.quantity, this.name});

  Items.fromJson(Map<String, dynamic> json) {
    productImg = json['productImg'];
    productId = json['productId'];
    price = json['price'];
    quantity = json['quantity'];
    name = json['name'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['productImg'] = this.productImg;
    data['productId'] = this.productId;
    data['price'] = this.price;
    data['quantity'] = this.quantity;
    data['name'] = this.name;
    return data;
  }
}
