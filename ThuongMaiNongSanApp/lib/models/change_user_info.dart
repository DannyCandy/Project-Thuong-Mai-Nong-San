class ChangeUserInfoModel {
  final String userId;
  final String username;
  final String tenKhachHang;
  final String diaChi;
  final String phoneNumber;

  const ChangeUserInfoModel({
    required this.userId,
    required this.username,
    required this.tenKhachHang,
    required this.diaChi,
    required this.phoneNumber
  });

  Map<String, dynamic> toJson() {
    return {
      "userId": userId,
      "userName": username,
      "tenKhachHang": tenKhachHang,
      "diaChi": diaChi,
      "phone": phoneNumber,
    };
  }
}