class LoginModel {
  final String userNameOrEmail;
  final String password;

  const LoginModel({
    required this.userNameOrEmail,
    required this.password,
  });

  Map<String, dynamic> toJson() {
    return {
      'userNameOrEmail': userNameOrEmail,
      'password': password,
    };
  }
}