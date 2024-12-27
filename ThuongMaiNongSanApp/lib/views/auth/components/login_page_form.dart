import 'package:apptmns/controllers/save_controller.dart';
import 'package:apptmns/controllers/user_controller.dart';
import 'package:apptmns/logic_constants/api_endpoints.dart';
import 'package:apptmns/services/dio_db_connect.dart';
import 'package:apptmns/services/user_info_store.dart';
import 'package:apptmns/views/auth/dialogs/no_verified_dialogs.dart';
import 'package:dio/dio.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/flutter_svg.dart';
import 'package:get/get.dart';
import 'package:jwt_decoder/jwt_decoder.dart';
import 'package:loading_animation_widget/loading_animation_widget.dart';

import '../../../core/constants/constants.dart';
import '../../../core/routes/app_routes.dart';
import '../../../core/themes/app_themes.dart';
import '../../../core/utils/validators.dart';
import '../../../models/login_model.dart';
import '../../entrypoint/entrypoint_ui.dart';
import 'login_button.dart';

class LoginPageForm extends StatefulWidget {
  const LoginPageForm({
    super.key,
  });

  @override
  State<LoginPageForm> createState() => _LoginPageFormState();
}

class _LoginPageFormState extends State<LoginPageForm> {
  final _key = GlobalKey<FormState>();
  final TextEditingController _emailOrUsernameController = TextEditingController();
  final TextEditingController _passwordController = TextEditingController();
  bool isPasswordShown = false;
  bool _isLoading = false;

  @override
  void dispose() {
    _emailOrUsernameController.dispose();
    _passwordController.dispose();
    super.dispose();
  }

  onPassShowClicked() {
    setState(() {
      isPasswordShown = !isPasswordShown;
    });
  }

  Future<void> onLogin() async {
    final bool isFormOkay = _key.currentState?.validate() ?? false;
    /*String dioErrMessage = 'Xảy ra lỗi nhưng không có mô tả cụ thể';*/
    try{
      if (isFormOkay) {
        setState(() {
          _isLoading = true;
        });
        final loginData = LoginModel(
          userNameOrEmail: _emailOrUsernameController.text,
          password: _passwordController.text,
        );
        /*final http = ApiEndpoints.makeNoSSLSecureRequest();
      var result = await http.post(
        Uri.parse(ApiEndpoints.login),
        headers: {
          'Content-Type': 'application/json; charset=UTF-8',
        },
        body: jsonEncode(loginData.toJson()),
      );*/
        final DioClient dioClient = DioClient();
        var result = await dioClient.dio.post(ApiEndpoints.login,
            options: Options(
                headers: {
                  'Content-Type': 'application/json; charset=UTF-8',
                }
            ),
            data: loginData.toJson());

        if (result.statusCode == 200) {
          /*final jsonData = await json.decode(result.data);*/
          final jsonData = result.data;

          UserPreferences.setUserId(jsonData['userId']);
          UserPreferences.setUsername(jsonData['username']);
          UserPreferences.setUserEmail(jsonData['email']);
          UserPreferences.setUserFullName(jsonData['name']);
          UserPreferences.setUserAvatar(jsonData['userAvatar']);
          UserPreferences.setUserAddress(jsonData['diachi']);
          UserPreferences.setUserPhone(jsonData['phone'] ?? '');

          UserPreferences.setUserToken(jsonData['token']);
          UserPreferences.setUserRefreshToken(jsonData['refreshToken']);

          final decodedToken = JwtDecoder.decode(UserPreferences.getUserToken()!);

          String? role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
          if (!mounted) return;

          if (role!.isNotEmpty) {
            Get.find<SaveController>().clearFavourite();
            Get.lazyPut(()=>UserController());
            Get.find<UserController>().initUserInfo();
            Navigator.pushAndRemoveUntil(
              context,
              MaterialPageRoute(builder: (_) => const EntryPointUI()),
                  (Route<dynamic> route) => false,
            );
          }
        }
      }
    }on DioException catch (dioEx){
      String errMess = 'Có lỗi xảy ra nhưng không có mô tả chi tiết';
      if(dioEx.response != null){
        if(dioEx.response?.data != null){
          errMess = dioEx.response?.data['message'] ?? errMess;
        }
      }
      Get.dialog(
          NoVerifiedDialogs(message: errMess)
      );
    }catch(e){
      return;
    }finally{
      setState(() {
        _isLoading = false; // Ẩn loading khi kết thúc request
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Theme(
      data: AppTheme.defaultTheme.copyWith(
        inputDecorationTheme: AppTheme.secondaryInputDecorationTheme,
      ),
      child: Padding(
        padding: const EdgeInsets.all(AppDefaults.padding),
        child: Form(
          key: _key,
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              // Phone Field
              const Text("Email hoặc tên đăng nhập"),
              const SizedBox(height: 8),
              TextFormField(
                controller: _emailOrUsernameController,
                keyboardType: TextInputType.emailAddress,
                validator: (value) {
                  if (value == null || value.isEmpty) {
                    return 'Trường này là bắt buộc';
                  }
                  // Checking valid email
                  if (value.contains('@')) {
                    if(!Validators.email.isValid(value)){
                      return 'Hãy nhập email hợp lệ';
                    };
                  }
                  return null;
                },
                textInputAction: TextInputAction.next,
              ),
              const SizedBox(height: AppDefaults.padding),

              // Password Field
              const Text("Mật khẩu"),
              const SizedBox(height: 8),
              TextFormField(
                controller: _passwordController,
                validator: Validators.password.call,
                onFieldSubmitted: (v) => onLogin(),
                textInputAction: TextInputAction.done,
                obscureText: !isPasswordShown,
                decoration: InputDecoration(
                  suffixIcon: Material(
                    color: Colors.transparent,
                    child: IconButton(
                      onPressed: onPassShowClicked,
                      icon: SvgPicture.asset(
                        AppIcons.eye,
                        width: 24,
                      ),
                    ),
                  ),
                ),
              ),

              // Forget Password labelLarge
              Align(
                alignment: Alignment.centerRight,
                child: TextButton(
                  onPressed: () {
                    Navigator.pushNamed(context, AppRoutes.forgotPassword);
                  },
                  child: const Text('Quên mật khẩu?'),
                ),
              ),

              // Login labelLarge
              _isLoading ? Center(child: LoadingAnimationWidget.staggeredDotsWave(color: AppColors.primary, size: 150))
               :
              LoginButton(onPressed: () async {
                await onLogin();
              }),
            ],
          ),
        ),
      ),
    );
  }
}
