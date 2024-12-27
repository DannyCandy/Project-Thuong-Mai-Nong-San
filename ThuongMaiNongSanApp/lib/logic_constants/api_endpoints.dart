import 'dart:io';
import 'package:http/io_client.dart';

class ApiEndpoints {
  static const String apiUri = "https://nongsan.somee.com";
  /*"https://nongsan.somee.com"*/
  static const String login = "$apiUri/api/app/authorization/login";

  static const String register = "$apiUri/api/app/authorization/registration";

  static const String verifyRegisterOtp = "$apiUri/api/app/authorization/verifyOtp";

  static const String resendRegisterOtp = "$apiUri/api/app/authorization/resendOtp";

  static const String refreshToken = "$apiUri/api/app/user/refreshToken";

  static const String logout = "$apiUri/api/app/user/logout";

  static const String changePassword = "$apiUri/api/app/user/changePassword";

  static const String updateAvatar = "$apiUri/api/app/user/updateAvatar";

  static const String updateUserInfo = "$apiUri/api/app/user/updateUserInfo";

  static const String changeUserEmail = "$apiUri/api/app/user/changeEmail";

  static const String getUserProfile = "$apiUri/api/app/user/getUserProfile";

  static const String sendOTPResetPassword = "$apiUri/api/app/authorization/sendOTPResetPassword";

  static const String resetPassword = "$apiUri/api/app/authorization/resetPassword";

  static const String getAllUserAddress = "$apiUri/api/app/deliveryAddress/getAllAddresses";

  static const String addNewUserAddress = "$apiUri/api/app/deliveryAddress/createAddress";

  static const String updateUserAddress = "$apiUri/api/app/deliveryAddress/updateAddress";

  static const String deleteUserAddress = "$apiUri/api/app/deliveryAddress/deleteAddress";

  static const String getAllProducts = "$apiUri/api/app/product/getAllProducts";

  static const String getAllCategories = "$apiUri/api/app/product/getAllCategories";

  static const String getProductByPaginated = "$apiUri/api/app/product/getProductByPaginated";

  static const String getProductById = "$apiUri/api/app/product/getProductById";

  static const String checkOut = "$apiUri/api/app/order/checkout";

  static const String getOrderHistory = "$apiUri/api/app/order/getOrderHistory";

  static const String searchSuggestions = "$apiUri/api/app/product/searchSuggestions";


  static IOClient makeNoSSLSecureRequest(){
    final ioc = HttpClient();
    ioc.badCertificateCallback = (X509Certificate cert, String host, int port) => true;
    final http = IOClient(ioc);
    return http;
  }
}

