class Product {
  String? idsp;
  String? spName;
  String? hinhAnhSp;
  int? price;
  String? categoryName;

  Product(
      {this.idsp, this.spName, this.hinhAnhSp, this.price, this.categoryName});

  Product.fromJson(Map<String, dynamic> json) {
    idsp = json['idsp'];
    spName = json['spName'];
    hinhAnhSp = json['hinhAnhSp'];
    price = json['price'];
    categoryName = json['categoryName'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['idsp'] = this.idsp;
    data['spName'] = this.spName;
    data['hinhAnhSp'] = this.hinhAnhSp;
    data['price'] = this.price;
    data['categoryName'] = this.categoryName;
    return data;
  }
}
