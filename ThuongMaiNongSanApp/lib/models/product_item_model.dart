class ProductItemModel {
  String? idsp;
  String? spName;
  String? moTa;
  String? congDung;
  String? thanhPhanDinhDuong;
  String? hdsd;
  int? soLuongSp;
  String? hinhAnhSp;
  int? price;
  String? categoryName;
  String? idDaiLy;
  String? daiLyName;
  String? moTaCN;
  String? hinhAnhCN;

  ProductItemModel(
      {this.idsp,
        this.spName,
        this.moTa,
        this.congDung,
        this.thanhPhanDinhDuong,
        this.hdsd,
        this.soLuongSp,
        this.hinhAnhSp,
        this.price,
        this.categoryName,
        this.idDaiLy,
        this.daiLyName,
        this.moTaCN,
        this.hinhAnhCN});

  ProductItemModel.fromJson(Map<String, dynamic> json) {
    idsp = json['idsp'];
    spName = json['spName'];
    moTa = json['moTa'];
    congDung = json['congDung'];
    thanhPhanDinhDuong = json['thanhPhanDinhDuong'];
    hdsd = json['hdsd'];
    soLuongSp = json['soLuongSp'];
    hinhAnhSp = json['hinhAnhSp'];
    price = json['price'];
    categoryName = json['categoryName'];
    idDaiLy = json['idDaiLy'];
    daiLyName = json['daiLyName'];
    moTaCN = json['moTaCN'];
    hinhAnhCN = json['hinhAnhCN'];
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = new Map<String, dynamic>();
    data['idsp'] = this.idsp;
    data['spName'] = this.spName;
    data['moTa'] = this.moTa;
    data['congDung'] = this.congDung;
    data['thanhPhanDinhDuong'] = this.thanhPhanDinhDuong;
    data['hdsd'] = this.hdsd;
    data['soLuongSp'] = this.soLuongSp;
    data['hinhAnhSp'] = this.hinhAnhSp;
    data['price'] = this.price;
    data['categoryName'] = this.categoryName;
    data['idDaiLy'] = this.idDaiLy;
    data['daiLyName'] = this.daiLyName;
    data['moTaCN'] = this.moTaCN;
    data['hinhAnhCN'] = this.hinhAnhCN;
    return data;
  }
}
