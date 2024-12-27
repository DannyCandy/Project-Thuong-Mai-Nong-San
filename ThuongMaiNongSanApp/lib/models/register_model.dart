
class RegisterModel {
  final String userName;
  final String tenKhachHang;
  final String diaChi;
  final String email;
  final String password;
  final String role;

  const RegisterModel({
    required this.userName,
    required this.tenKhachHang,
    required this.diaChi,
    required this.email,
    required this.password,
    required this.role
  });

  Map<String, dynamic> toJson() {
    return {
      "userName": userName,
      "tenKhachHang": tenKhachHang,
      "diaChi": diaChi,
      "role": role,
      "email": email,
      "password": password,
    };
  }
}