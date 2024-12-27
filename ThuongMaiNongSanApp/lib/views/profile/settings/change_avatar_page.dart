import 'dart:convert';
import 'dart:io';

import 'package:apptmns/services/user_info_store.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';
import 'package:image_handler/image_handler.dart';
import 'package:loading_animation_widget/loading_animation_widget.dart';
import '../../../controllers/user_controller.dart';
import '../../../core/components/app_back_button.dart';
import '../../../core/constants/app_colors.dart';
import '../../../core/constants/app_defaults.dart';



class AvatarEditPage extends StatefulWidget {
  const AvatarEditPage({super.key});

  @override
  State<AvatarEditPage> createState() => _AvatarEditPageState();
}

class _AvatarEditPageState extends State<AvatarEditPage> {

  XFile? _image; // Để lưu trữ ảnh đại diện
  final ImagePicker _picker = ImagePicker();
  String userId = UserPreferences.getUserId()!;
  /*bool _isLoading = false;*/

  final UserController _profileController = Get.put(UserController());
  @override
  void dispose(){
    super.dispose();
  }

  Future<void> _pickImage() async {
    final XFile? image = await _picker.pickImage(source: ImageSource.gallery);
    if(image != null){
      int fileSize =  await ImageHandler.checkCompressedFileSize(image);
      if(fileSize > 5242880){
        if(mounted){
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(content: Text("Hình ảnh quá lớn, vượt quá 5MB", style: TextStyle(color: AppColors.gray),),
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
          const SnackBar(content: Text("Không hỗ trợ tệp này.")),
        );
      }
    }
  }

  Future<void> onUpdateAvatar() async {
    await _profileController.updateUserAvatar(_image, userId);
    /*setState(() {
      _isLoading = true;
    });*/
    /*final bool isSelectedImage = _image != null;
    if (isSelectedImage) {
      final url = Uri.parse(ApiEndpoints.updateAvatar);
      final httpClient = ApiEndpoints.makeNoSSLSecureRequest();

      final request = http.MultipartRequest('POST', url);

      request.headers['Authorization'] = 'Bearer ${UserPreferences.getUserToken()}'; // Thay $yourToken bằng token của bạn
      request.headers['Content-Type'] = 'multipart/form-data';

      request.fields['idUser'] = userId; // id người dùng
      //Xử lý đối tượng ảnh
      Uint8List bytes = await _image!.readAsBytes();
      var file = http.MultipartFile(
        'avatar', // Tên trường file trong yêu cầu
        http.ByteStream.fromBytes(bytes),
        bytes.length, // Đường dẫn đến file ảnh
        filename: _image!.name,
      );
      request.files.add(file);

      var result = await httpClient.send(request);
      String responseBody = await result.stream.bytesToString();

      dynamic infoResponse;

      if (result.statusCode == 200) {
        // Kiểm tra nếu body không rỗng trước khi giải mã
        if (responseBody.isNotEmpty) {
          infoResponse = jsonDecode(responseBody);
        }
        if (!mounted) return;
        void turnBack(){
          Navigator.pop(context);
        }
        showGeneralDialog(
          barrierLabel: 'Success',
          barrierDismissible: true,
          context: context,
          pageBuilder: (ctx, anim1, anim2) => SuccessDialog(onPressed: turnBack, message: infoResponse['message'], buttonTitle: 'OK'),
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
            'message': 'Lỗi trong quá trình cập nhật avatar.'
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
      if (!mounted) return;
      void noUpdate(){
        Navigator.pop(context);
      }
      showGeneralDialog(
        barrierLabel: 'Info',
        barrierDismissible: true,
        context: context,
        pageBuilder: (ctx, anim1, anim2) => InfoDialog(onPressed: noUpdate,message: 'Không có thay đổi gì!',buttonTitle: 'Ok nha'),
        transitionBuilder: (ctx, anim1, anim2, child) => ScaleTransition(
          scale: anim1,
          child: child,
        ),
      );
    }*/
    /*setState(() {
      _isLoading = false;
    });*/
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.cardColor,
      appBar: AppBar(
        leading: const AppBackButton(),
        title: const Text(
          'Cập nhật ảnh đại diện',
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
          child: Column(
            mainAxisSize: MainAxisSize.min,
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              /* <----  First Name -----> */
              const Center(child: Text("Ảnh đại diện")),
              const SizedBox(height: AppDefaults.padding*2),
              GestureDetector(
                onDoubleTap: (){
                  _pickImage();
                },
                child: Container(
                  child: _image == null ?
                  Obx(() =>
                      Center(
                        child: Image.memory(
                          base64Decode(_profileController.listUserInfo['avatar']),
                          width: MediaQuery.of(context).size.width * 0.6,
                          height: MediaQuery.of(context).size.width * 0.6,
                          fit: BoxFit.cover,
                        ),
                      ))
                  /*Center(
                    child: Image.asset(
                      'assets/images/defaultImg.png',
                      height: 200,
                      width: 200,
                      fit: BoxFit.cover,
                    ),
                  )*/
                      :
                  Center(
                    child: Image.file(
                      File(_image!.path).absolute,
                      height: MediaQuery.of(context).size.width * 0.6,
                      width: MediaQuery.of(context).size.width * 0.6,
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
              SizedBox(
                width: double.infinity,
                child: Obx(() =>
                _profileController.isLoading.value ? Center(child: LoadingAnimationWidget.staggeredDotsWave(color: AppColors.primary, size: 150))
                    :
                ElevatedButton(
                  onPressed: () async {
                    await onUpdateAvatar();
                  },
                  child: const Text('Cập nhật ảnh'),
                ),)
              ),
            ],
          ),
        ),
      ),
    );
  }
}
