import 'package:form_field_validator/form_field_validator.dart';

class Validators {
  /// Email Validator
  static final email = EmailValidator(errorText: 'Email không hợp lệ.');
  static final registerEmail = MultiValidator([
    RequiredValidator(errorText: 'Email là bắt buộc'),
    EmailValidator(errorText: 'Hãy nhập email hợp lệ')]);
  /// Username Validator
  static final username = MultiValidator([
    PatternValidator(r'^[a-zA-Z0-9]+$', errorText: 'Tên đăng nhập chỉ được chứa ký tự và chữ số'),
    RequiredValidator(errorText: 'Tên đăng nhập là bắt buộc')
  ]);
  /// Phone Validator
  static final phone = MultiValidator([
    PatternValidator(r'^((03[2-9]|05[6|8|9]|07[0|6|7|8|9]|08[1-9]|09[1-4|6-9])[0-9]{7})$', errorText: 'Số điện thoại Việt Nam phải chứa 10-11 chữ số'),
    RequiredValidator(errorText: 'Số điện thoại là bắt buộc')
  ]);

  /// Password Validator
  static final password = MultiValidator([
    RequiredValidator(errorText: 'Mật khẩu là bắt buộc'),
    MinLengthValidator(6, errorText: 'Mật khẩu phải có ít nhất 6 ký tự'),
    PatternValidator(r'(?=.*?[#?!@$%^&*-])', errorText: 'Mật khẩu phải có ít nhất 1 ký tự đặc biệt'),
    /// New code
    PatternValidator(r'(?=.*?[A-Z])', errorText: 'Mật khẩu phải có ít nhất 1 ký tự in hoa'),
    PatternValidator(r'(?=.*?[a-z])', errorText: 'Mật khẩu phải có ít nhất 1 ký tự in thường'),
    PatternValidator(r'(?=.*?[0-9])', errorText: 'Mật khẩu phải có ít nhất 1 ký tự số'),
  ]);

  /// Required Validator with Optional Field Name
  static RequiredValidator requiredWithFieldName(String? fieldName) =>
      RequiredValidator(errorText: '${fieldName ?? 'Trường này'} là bắt buộc');

  /// Plain Required Validator
  static final required = RequiredValidator(errorText: 'Trường này là bắt buộc');
}
