import 'dart:convert';
import 'dart:io';
import 'package:apptmns/views/auth/dialogs/info_dialogs.dart';
import 'package:apptmns/views/auth/dialogs/success_dialogs.dart';
import 'package:apptmns/views/auth/number_verification_page.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_svg/svg.dart';
import 'package:get/get.dart';
import 'package:image_handler/image_handler.dart';
import 'package:http/http.dart' as http;
import 'package:loading_animation_widget/loading_animation_widget.dart';
import '../../../logic_constants/api_endpoints.dart';
import '../../../logic_constants/image_handler.dart';
import '../../../logic_constants/input_border_styles.dart';
import '../../../core/constants/constants.dart';
import '../../../core/routes/app_routes.dart';
import '../../../core/utils/validators.dart';
import '../dialogs/no_verified_dialogs.dart';
import 'already_have_accout.dart';
import 'sign_up_button.dart';

class SignUpForm extends StatefulWidget {
  const SignUpForm({
    super.key,
  });

  @override
  State<SignUpForm> createState() => _SignUpFormState();
}

class _SignUpFormState extends State<SignUpForm> {
  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();
  final TextEditingController _emailController = TextEditingController();
  final TextEditingController _passwordController = TextEditingController();
  final TextEditingController _nameController = TextEditingController();
  final TextEditingController _addressController = TextEditingController();
  final TextEditingController _userNameController = TextEditingController();

  XFile? _image; // Để lưu trữ ảnh đại diện
  final ImagePicker _picker = ImagePicker();
  List<String> roles = ['User', 'Manager']; // Danh sách các role
  String? selectedRole; // Lưu trữ role được chọn
  bool _isLoading = false;
  bool isPasswordShown = false;

  @override
  void dispose() {
    _emailController.dispose();
    _passwordController.dispose();
    _nameController.dispose();
    _addressController.dispose();
    _userNameController.dispose();
    super.dispose();
  }

  Future<void> _pickImage() async {
    final XFile? image = await _picker.pickImage(source: ImageSource.gallery);
    if(image != null){
      int fileSize =  await ImageHandler.checkCompressedFileSize(image);
      if(fileSize > 5242880){
        if(mounted){
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(content: Text("Hình ảnh quá lớn, đã vượt quá 5MB", style: TextStyle(color: AppColors.gray),),
              backgroundColor: AppColors.primary,),
          );
        }
      }else{
        setState(() {
          _image = image; // Cập nhật ảnh đại diện
        });
      }
    }else{
      if(mounted){
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text("Không hỗ trợ loại tệp này.")),
        );
      }
    }
  }

  onPassShowClicked() {
    setState(() {
      isPasswordShown = !isPasswordShown;
    });
  }

  Future<void> onRegister() async {
    final bool isFormOkay = _formKey.currentState?.validate() ?? false;
    if (isFormOkay) {
      setState(() {
        _isLoading = true;
      });
      String avatarImagePath = _image?.path ?? 'assets/images/defaultImg.png'; // Sử dụng ảnh mặc định nếu không có ảnh nào được chọn

      final url = Uri.parse(ApiEndpoints.register);
      final httpClient = ApiEndpoints.makeNoSSLSecureRequest();

      final request = http.MultipartRequest('POST', url);

      request.headers['Content-Type'] = 'multipart/form-data';

      request.fields['username'] = _userNameController.text; // Tên người dùng
      request.fields['tenKhachHang'] = _nameController.text; // Tên khách hàng
      request.fields['diaChi'] = _addressController.text; // Địa chỉ
      request.fields['email'] = _emailController.text; // Email
      request.fields['password'] = _passwordController.text; // Mật khẩu
      request.fields['role'] = selectedRole!; // Vai trò

      //Xử lý đối tượng ảnh
      if(_image != null){
        Uint8List bytes = await _image!.readAsBytes();
        var file = http.MultipartFile(
          'avatar', // Tên trường file trong yêu cầu
          http.ByteStream.fromBytes(bytes),
          bytes.length, // Đường dẫn đến file ảnh
          filename: _image!.name,
        );
        request.files.add(file);
      }else{
        Uint8List bytes = await ImageToByte.assetToXFile(avatarImagePath);
        var file = http.MultipartFile(
          'avatar', // Tên trường file trong yêu cầu
          http.ByteStream.fromBytes(bytes), // Tạo ByteStream từ bytes
          bytes.length, // Độ dài của dữ liệu
          filename: 'defaultAvatar.png', // Đặt tên file
        );
        request.files.add(file);
      }

      var result = await httpClient.send(request);
      String responseBody = await result.stream.bytesToString();

      setState(() {
        _isLoading = false;
      });

      dynamic infoResponse;
      if (result.statusCode == 200) {
        // Kiểm tra nếu body không rỗng trước khi giải mã
        if (responseBody.isNotEmpty) {
          infoResponse = jsonDecode(responseBody);
        } else {
          infoResponse = {
            'statusCode': result.statusCode,
            'message': 'Lỗi trong quá trình tạo mã Otp.'
          };
        }
        if (!mounted) return;
        void toOtpPage(){
          /*Navigator.pushNamed(context, AppRoutes.numberVerification);*/
          Get.to(() => NumberVerificationPage(userEmail: _emailController.text));
        }
        showGeneralDialog(
          barrierLabel: 'Info',
          barrierDismissible: true,
          context: context,
          pageBuilder: (ctx, anim1, anim2) => SuccessDialog(onPressed: toOtpPage,message: infoResponse['message'],buttonTitle: 'Ok'),
          transitionBuilder: (ctx, anim1, anim2, child) => ScaleTransition(
            scale: anim1,
            child: child,
          ),
        );
      } else {
        // Kiểm tra nếu body không rỗng trước khi giải mã
        if (responseBody.isNotEmpty) {
          infoResponse = jsonDecode(responseBody);
        } else {
          infoResponse = {
            'statusCode': result.statusCode,
            'message': 'Có lỗi xảy ra, \nnhưng không có mô tả chi tiết.'
          };
        }
        if (!mounted) return;
        showGeneralDialog(
          barrierLabel: 'Error',
          barrierDismissible: true,
          context: context,
          pageBuilder: (ctx, anim1, anim2) => NoVerifiedDialogs(message: infoResponse['message']),
          transitionBuilder: (ctx, anim1, anim2, child) => ScaleTransition(
            scale: anim1,
            child: child,
          ),
        );
      }
    } else {
      return;
    }
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      margin: const EdgeInsets.all(AppDefaults.margin),
      padding: const EdgeInsets.all(AppDefaults.padding),
      decoration: BoxDecoration(
        color: Colors.white,
        boxShadow: AppDefaults.boxShadow,
        borderRadius: AppDefaults.borderRadius,
      ),
      child: Form(
        key: _formKey,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            const Text("Tên đầy đủ"),
            const SizedBox(height: 8),
            TextFormField(
              controller: _nameController,
              validator: Validators.requiredWithFieldName('Tên đầy đủ').call,
              textInputAction: TextInputAction.next,
            ),
            const SizedBox(height: AppDefaults.padding),
            // const Text("Phone Number"),
            // const SizedBox(height: 8),
            // TextFormField(
            //   textInputAction: TextInputAction.next,
            //   validator: Validators.required.call,
            //   keyboardType: TextInputType.number,
            //   inputFormatters: [FilteringTextInputFormatter.digitsOnly],
            // ),
            // const SizedBox(height: AppDefaults.padding),
            const Text("Tên đăng nhập"),
            const SizedBox(height: 8),
            TextFormField(
              controller: _userNameController,
              validator: Validators.requiredWithFieldName('Tên đăng nhập').call,
              textInputAction: TextInputAction.next,
            ),
            const SizedBox(height: AppDefaults.padding),
            const Text("Email"),
            const SizedBox(height: 8),
            TextFormField(
              controller: _emailController,
              validator: Validators.registerEmail.call,
              textInputAction: TextInputAction.next,
            ),
            const SizedBox(height: AppDefaults.padding),
            const Text("Mật khẩu"),
            const SizedBox(height: 8),
            TextFormField(
              controller: _passwordController,
              validator: Validators.password.call,
              textInputAction: TextInputAction.next,
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
            const SizedBox(height: AppDefaults.padding),
            const Text("Địa chỉ"),
            const SizedBox(height: 8),
            TextFormField(
              controller: _addressController,
              validator: Validators.requiredWithFieldName('Địa chỉ').call,
              textInputAction: TextInputAction.next,
            ),
            const SizedBox(height: AppDefaults.padding),
            //Box chứa ảnh
            GestureDetector(
              onDoubleTap: (){
                _pickImage();
              },
              child: Container(
                child: _image == null ?
                Center(
                  child: Image.asset(
                    'assets/images/defaultImg.png',
                    height: 200,
                    width: 200,
                    fit: BoxFit.cover,
                  ),
                )
                    :
                Center(
                  child: Image.file(
                    File(_image!.path).absolute,
                    height: 200,
                    width: 200,
                    fit: BoxFit.cover,
                  ),
                ),
              ),
            ),
            const SizedBox(height: AppDefaults.padding),
            Center(
              child: ElevatedButton(
                onPressed: _pickImage,
                style: ElevatedButton.styleFrom(elevation: 1),
                child: Text(_image == null ? 'Chọn ảnh' : 'Đã chọn ảnh'),
              ),
            ),
            const SizedBox(height: AppDefaults.padding),
            DropdownButtonFormField<String>(
              decoration: InputDecoration(
                labelText: 'Chọn quyền',
                floatingLabelStyle: const TextStyle(color: AppColors.primary),
                border: BorderStyles.border,
                focusedBorder: BorderStyles.focusedBorder,
                errorBorder: BorderStyles.errorBorder,
                focusedErrorBorder: BorderStyles.focusedErrorBorder,),
              value: selectedRole,
              items: roles.map((String role) {
                return DropdownMenuItem<String>(
                  value: role,
                  child: Text(role),
                );
              }).toList(),
              onChanged: (value) {
                setState(() {
                  selectedRole = value;
                });
              },
              validator: (value) {
                if (value == null) {
                  return 'Hãy chọn một quyền.';
                }
                return null;
              },
            ),
            const SizedBox(height: AppDefaults.padding),
            _isLoading ? Center(child: LoadingAnimationWidget.staggeredDotsWave(color: AppColors.primary, size: 150)) :
            SignUpButton(onPressed: () async {
              await onRegister();
            }),
            const AlreadyHaveAnAccount(),
            const SizedBox(height: AppDefaults.padding),
          ],
        ),
      ),
    );
  }
}
