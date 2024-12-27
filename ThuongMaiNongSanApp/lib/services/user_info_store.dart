import 'package:shared_preferences/shared_preferences.dart';

class UserPreferences {
  static SharedPreferences? _preferences;

  static const _KEY_USER_NAME = 'user_name';
  static const _KEY_USER_ID = 'user_id';
  static const _KEY_USER_EMAIL = 'user_email';

  static const _KEY_USER_FULL_NAME = 'user_full_name';
  static const _KEY_USER_AVATAR = 'user_avatar';
  static const _KEY_USER_ADDRESS = 'user_address';
  static const _KEY_USER_PHONE = 'user_phone';

  static const _KEY_USER_TOKEN = 'user_token';
  static const _KEY_USER_REFRESH_TOKEN = 'user_refresh_token';

  static Future init() async {
    _preferences = await SharedPreferences.getInstance();
  }

  static Future clearData() async {
    await _preferences?.clear();
  }

  //Username
  static Future setUsername(String username) async {
    await _preferences?.setString(_KEY_USER_NAME, username);
  }

  static String? getUsername() {
    return _preferences?.getString(_KEY_USER_NAME);
  }

  //UserID
  static Future setUserId(String userid) async {
    await _preferences?.setString(_KEY_USER_ID, userid);
  }

  static String? getUserId() {
    return _preferences?.getString(_KEY_USER_ID);
  }

  //User email
  static Future setUserEmail(String email) async {
    await _preferences?.setString(_KEY_USER_EMAIL, email);
  }

  static String? getUserEmail() {
    return _preferences?.getString(_KEY_USER_EMAIL);
  }

  //User jwt token
  static Future setUserToken(String token) async {
    await _preferences?.setString(_KEY_USER_TOKEN, token);
  }

  static String? getUserToken() {
    return _preferences?.getString(_KEY_USER_TOKEN);
  }

  //User jwt fresh token
  static Future setUserRefreshToken(String refreshToken) async {
    await _preferences?.setString(_KEY_USER_REFRESH_TOKEN, refreshToken);
  }

  static String? getUserRefreshToken() {
    return _preferences?.getString(_KEY_USER_REFRESH_TOKEN);
  }

  //User full name
  static Future setUserFullName(String name) async {
    await _preferences?.setString(_KEY_USER_FULL_NAME, name);
  }

  static String? getUserFullName() {
    return _preferences?.getString(_KEY_USER_FULL_NAME);
  }

  //User base64String avatar
  static Future setUserAvatar(String base64String) async {
    await _preferences?.setString(_KEY_USER_AVATAR, base64String);
  }

  static String? getUserAvatar() {
    return _preferences?.getString(_KEY_USER_AVATAR);
  }

  //User address
  static Future setUserAddress(String address) async {
    await _preferences?.setString(_KEY_USER_ADDRESS, address);
  }

  static String? getUserAddress() {
    return _preferences?.getString(_KEY_USER_ADDRESS);
  }

  //User phone
  static Future setUserPhone(String phone) async {
    await _preferences?.setString(_KEY_USER_PHONE, phone);
  }

  static String? getUserPhone() {
    return _preferences?.getString(_KEY_USER_PHONE);
  }
}
