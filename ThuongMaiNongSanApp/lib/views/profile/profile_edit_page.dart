
import 'package:apptmns/core/utils/validators.dart';
import 'package:apptmns/models/change_user_info.dart';
import 'package:apptmns/services/user_info_store.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:loading_animation_widget/loading_animation_widget.dart';

import '../../controllers/user_controller.dart';
import '../../core/components/app_back_button.dart';
import '../../core/constants/constants.dart';

class ProfileEditPage extends StatefulWidget {
  const ProfileEditPage({super.key});
  @override
  State<ProfileEditPage> createState() => _ProfileEditPageState();
}

class _ProfileEditPageState extends State<ProfileEditPage> {
  final GlobalKey<FormState> _formKey = GlobalKey<FormState>();

  final TextEditingController _tenKhachHang = TextEditingController();
  final TextEditingController _username = TextEditingController();
  final TextEditingController _diaChi = TextEditingController();
  final TextEditingController _phoneNumber = TextEditingController();

  final UserController _profileController = Get.put(UserController());
  final String userId = UserPreferences.getUserId()!;

  @override
  void dispose(){
    _tenKhachHang.dispose();
    _username.dispose();
    _diaChi.dispose();
    _phoneNumber.dispose();
    super.dispose();
  }

  Future<void> onSubmitEditProfile() async {
    final bool isFormOkay = _formKey.currentState?.validate() ?? false;
    if (isFormOkay) {
      final editProfileData = ChangeUserInfoModel(
        userId: userId,
        username: _username.text,
        tenKhachHang: _tenKhachHang.text,
        diaChi: _diaChi.text,
        phoneNumber: _phoneNumber.text,
      );
      _username.clear();
      _tenKhachHang.clear();
      _diaChi.clear();
      _phoneNumber.clear();
      await _profileController.updateUserProfile(editProfileData);
    }
  }

  /*Future<void> onSubmitEditProfile() async {
    final bool isFormOkay = _formKey.currentState?.validate() ?? false;
    if (isFormOkay) {
      setState(() {
        _isLoading = true;
      });
      final editProfileData = ChangeUserInfoModel(
        userId: userId,
        username: _username.text,
        tenKhachHang: _tenKhachHang.text,
        diaChi: _diaChi.text,
        phoneNumber: _phoneNumber.text,
      );
      final http = ApiEndpoints.makeNoSSLSecureRequest();
      var result = await http.post(
        Uri.parse(ApiEndpoints.updateUserInfo),
        headers: {
          'Content-Type': 'application/json; charset=UTF-8',
          'Authorization': 'Bearer ${UserPreferences.getUserToken()}',
        },
        body: jsonEncode(editProfileData.toJson()),
      );

      setState(() {
        _isLoading = false; // Ẩn loading khi kết thúc request
      });

      if(result.statusCode == 200){
        await UserPreferences.setUserFullName(_tenKhachHang.text);
        if (!mounted) return;
        final jsonData = json.decode(result.body);
        void turnBack(){
          Navigator.pop(context);
        }
        showGeneralDialog(
          barrierLabel: 'DialogSuccess',
          barrierDismissible: true,
          context: context,
          pageBuilder: (ctx, anim1, anim2) => InfoDialog(onPressed: turnBack, message: jsonData['message'], buttonTitle: 'Trở về'),
          transitionBuilder: (ctx, anim1, anim2, child) => ScaleTransition(
            scale: anim1,
            child: child,
          ),
        );
      }else{
        String errorMessage = 'Có lỗi xảy ra khi cập nhật thông tin\nnhưng không có mô tả chi tiết.';
        if(result.body.isNotEmpty){
          final jsonData = json.decode(result.body);
          errorMessage = jsonData['message'];
        }
        if (!mounted) return;
        showGeneralDialog(
          barrierLabel: 'DialogError',
          barrierDismissible: true,
          context: context,
          pageBuilder: (ctx, anim1, anim2) => NoVerifiedDialogs(message: errorMessage),
          transitionBuilder: (ctx, anim1, anim2, child) => ScaleTransition(
            scale: anim1,
            child: child,
          ),
        );
      }
    }
  }*/

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.cardColor,
      appBar: AppBar(
        leading: const AppBackButton(),
        title: const Text(
          'Cập nhật thông tin người dùng',
        ),
      ),
      body: SingleChildScrollView(
        child: Container(
          margin: const EdgeInsets.all(AppDefaults.padding),
          padding: const EdgeInsets.symmetric(
            horizontal: AppDefaults.padding,
            vertical: AppDefaults.padding * 2,
          ),
          decoration: BoxDecoration(
            color: AppColors.scaffoldBackground,
            borderRadius: AppDefaults.borderRadius,
          ),
          child: Form(
            key: _formKey,
            child: Column(
              mainAxisSize: MainAxisSize.min,
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                /* <----  First Name -----> */
                const Text("Tên khách hàng"),
                const SizedBox(height: 8),
                Obx(() => TextFormField(
                  controller: _tenKhachHang,
                  keyboardType: TextInputType.text,
                  textInputAction: TextInputAction.next,
                  validator: Validators.requiredWithFieldName('Tên khách hàng').call,
                  decoration: InputDecoration(
                    hintText: _profileController.listUserInfo['tenKhachHang'], // Văn bản mờ mờ
                    hintStyle: const TextStyle(color: Colors.grey), // Đặt màu chữ cho hintText nếu muốn
                  ),
                ),),

                const SizedBox(height: AppDefaults.padding),

                /* <---- Last Name -----> */
                const Text("Tên đăng nhập"),
                const SizedBox(height: 8),
                Obx(() => TextFormField(
                  controller: _username,
                  keyboardType: TextInputType.text,
                  textInputAction: TextInputAction.next,
                  validator: Validators.username.call,
                  decoration: InputDecoration(
                    hintText: _profileController.listUserInfo['userName'], // Văn bản mờ mờ
                    hintStyle: const TextStyle(color: Colors.grey), // Đặt màu chữ cho hintText nếu muốn
                  ),
                ),),
                
                const SizedBox(height: AppDefaults.padding),

                /* <---- Phone Number -----> */
                const Text("Số điện thoại"),
                const SizedBox(height: 8),
                Obx(() => TextFormField(
                  controller: _phoneNumber,
                  keyboardType: TextInputType.number,
                  textInputAction: TextInputAction.next,
                  validator: Validators.requiredWithFieldName('Số điện thoại').call,
                  decoration: InputDecoration(
                    hintText: _profileController.listUserInfo['phone'], // Văn bản mờ mờ
                    hintStyle: const TextStyle(color: Colors.grey), // Đặt màu chữ cho hintText nếu muốn
                  ),
                ),),
                const SizedBox(height: AppDefaults.padding),
                /* <---- Gender -----> */
                const Text("Địa chỉ"),
                const SizedBox(height: 8),
                Obx(() => TextFormField(
                  controller: _diaChi,
                  keyboardType: TextInputType.text,
                  textInputAction: TextInputAction.next,
                  validator: Validators.requiredWithFieldName('Địa chỉ').call,
                  decoration: InputDecoration(
                    hintText: _profileController.listUserInfo['diaChi'], // Văn bản mờ mờ
                    hintStyle: const TextStyle(color: Colors.grey), // Đặt màu chữ cho hintText nếu muốn
                  ),
                ),),
                const SizedBox(height: AppDefaults.padding),

                /* <---- Submit -----> */
                const SizedBox(height: AppDefaults.padding),
                SizedBox(
                  width: double.infinity,
                  child:Obx(() =>
                  _profileController.isLoading.value ? Center(child: LoadingAnimationWidget.staggeredDotsWave(color: AppColors.primary, size: 150))
                      :
                  ElevatedButton(
                    onPressed: () async {
                      await onSubmitEditProfile();
                      _username.clear();
                      _phoneNumber.clear();
                      _diaChi.clear();
                      _tenKhachHang.clear();
                    },
                    child: const Text('Lưu'),
                  ),)
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
