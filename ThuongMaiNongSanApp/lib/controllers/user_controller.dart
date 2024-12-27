import 'dart:typed_data';

import 'package:apptmns/controllers/address_controller.dart';
import 'package:apptmns/controllers/cart_controller.dart';
import 'package:apptmns/controllers/order_history_controller.dart';
import 'package:apptmns/controllers/save_controller.dart';
import 'package:apptmns/services/user_info_store.dart';
import 'package:apptmns/views/auth/login_page.dart';
import 'package:dio/dio.dart' as dio;
import 'package:dio/dio.dart';
import 'package:get/get.dart';
import 'package:image_handler/image_handler.dart';
import 'package:jwt_decoder/jwt_decoder.dart';

import '../logic_constants/api_endpoints.dart';
import '../models/change_user_info.dart';
import '../services/dio_db_connect.dart';
import '../views/auth/dialogs/no_verified_dialogs.dart';
import '../views/auth/dialogs/success_dialogs.dart';

class UserController extends GetxController {
  RxMap<String, dynamic> listUserInfo = <String, dynamic>{}.obs;
  RxBool isLoading = false.obs;

  @override
  void onInit() async {
    super.onInit();
    initUserInfo();
  }

  void setIsLoading(bool value){
    isLoading.value = value;
  }

  Future<void> getAllUserInfo() async {
    setIsLoading(true);

    try{
      if(UserPreferences.getUserId() == null){
        Get.offAll(() => const LoginPage());
        return;
      }
      String? accessToken = UserPreferences.getUserToken();
      if(!JwtDecoder.isExpired(accessToken!)){
        initUserInfo();
        return;
      }
      final DioClient dioClient = DioClient();
      var result = await dioClient.dio.get(ApiEndpoints.getUserProfile,
        options: dio.Options(
          headers: {
            'Content-Type': 'application/json; charset=UTF-8',
          }
        ),
        queryParameters: {
          'userId':UserPreferences.getUserId(),
        }
      );

      if(result.statusCode == 200){
        final dataCasted = result.data;
        UserPreferences.setUserFullName(dataCasted['tenKhachHang']);
        UserPreferences.setUserEmail(dataCasted['email']);
        UserPreferences.setUserAvatar(dataCasted['avatar']);
        UserPreferences.setUserPhone(dataCasted['phone']);
        UserPreferences.setUserAddress(dataCasted['diaChi']);
        UserPreferences.setUsername(dataCasted['userName']);
        listUserInfo.assignAll(dataCasted);
      }
    }
    on dio.DioException catch (dioEx){
      String errMess = 'Phiên đăng nhập của bạn đã hết hạn, vui lòng đăng nhập lại';
      if(dioEx.response != null){
        if(dioEx.response?.data is Map<String, dynamic>){
          errMess = dioEx.response?.data['message'];
        }
      }
      Get.dialog(
        NoVerifiedDialogs(message: errMess)
      );
    }catch(e){
      return;
    }finally{
      setIsLoading(false);
    }
  }

  void initUserInfo(){
    listUserInfo.assignAll({
      'tenKhachHang': UserPreferences.getUserFullName() ?? 'Không có dữ liệu',
      'email': UserPreferences.getUserEmail() ?? 'Không có dữ liệu',
      'avatar': UserPreferences.getUserAvatar() ?? '',
      'phone': UserPreferences.getUserPhone() ?? 'Không có dữ liệu',
      'diaChi': UserPreferences.getUserAddress() ?? 'Không có dữ liệu',
      'userName': UserPreferences.getUsername() ?? 'Không có dữ liệu',
    });
  }

  void resignUserInfo(String key, String value){
    listUserInfo.update(key, (_) => value);
  }

  Future<void> updateUserProfile(ChangeUserInfoModel editProfileData) async {
    setIsLoading(true);
    final DioClient dioClient = DioClient();
    try{
      final result = await dioClient.dio.post(ApiEndpoints.updateUserInfo,
        options: dio.Options(
            headers: {
              'Content-Type': 'application/json; charset=UTF-8',
            }
        ),
        data: editProfileData.toJson(),
      );


      if (result.statusCode == 200) {
        final jsonData = result.data;
        UserPreferences.setUserFullName(editProfileData.tenKhachHang);
        UserPreferences.setUsername(editProfileData.username);
        UserPreferences.setUserPhone(editProfileData.phoneNumber);
        UserPreferences.setUserAddress(editProfileData.diaChi);
        resignUserInfo('tenKhachHang', editProfileData.tenKhachHang);
        resignUserInfo('userName', editProfileData.username);
        resignUserInfo('phone', editProfileData.phoneNumber);
        resignUserInfo('diaChi', editProfileData.diaChi);
        Get.dialog(
          SuccessDialog(
            onPressed: () => Get.back(),
            message: jsonData['message'],
            buttonTitle: 'OK',
          ),
        );
      }
    }on dio.DioException catch (dioEx){
      String errMess = 'Có lỗi xảy ra nhưng không có mô tả chi tiết';
      if(dioEx.response != null){
        if(dioEx.response?.data is Map<String, dynamic>){
          errMess = dioEx.response?.data['message'];
        }
      }
      Get.dialog(
          NoVerifiedDialogs(message: errMess)
      );
    }catch(e){
      return;
    }finally{
      setIsLoading(false);
    }
  }

  Future<void> updateUserEmail(Map<String,dynamic> editEmailData) async {
    setIsLoading(true);
    try{
      final DioClient dioClient = DioClient();
      var result = await dioClient.dio.post(ApiEndpoints.changeUserEmail,
        options: dio.Options(
            headers: {
              'Content-Type': 'application/json; charset=UTF-8',
            }
        ),
        data: editEmailData,
      );


      if (result.statusCode == 200) {
        final jsonData = result.data;
        UserPreferences.setUserEmail(editEmailData['email']);
        resignUserInfo('email', editEmailData['email']);
        Get.dialog(
          SuccessDialog(
            onPressed: () => Get.back(),
            message: jsonData['message'],
            buttonTitle: 'OK',
          ),
        );
      }
    }on dio.DioException catch (dioEx){
      String errMess = 'Có lỗi xảy ra nhưng không có mô tả chi tiết';
      if(dioEx.response != null){
        if(dioEx.response?.data is Map<String, dynamic>){
          errMess = dioEx.response?.data['message'];
        }
      }
      Get.dialog(
          NoVerifiedDialogs(message: errMess)
      );
    }catch(e){
      return;
    }finally{
      setIsLoading(false);
    }
  }

  Future<void> updateUserAvatar(XFile? image, String userId) async {
    setIsLoading(true);
    try{
      final DioClient dioClient = DioClient();
      final bool isSelectedImage = image != null;

      if (isSelectedImage) {
        // Xử lý đối tượng ảnh
        Uint8List bytes = await image.readAsBytes();
        dio.FormData formData = dio.FormData.fromMap({
          'idUser': userId,
          'avatar': dio.MultipartFile.fromBytes(
            bytes,
            filename: image.name,
          ),
        });

        final result = await dioClient.dio.post(ApiEndpoints.updateAvatar,
            options: dio.Options(
                headers: {
                  'Content-Type': 'multipart/form-data',
                }
            ),
            data: formData);

        Map<String,dynamic> infoResponse = result.data;

        if (result.statusCode == 200) {
          resignUserInfo('avatar', infoResponse['userAvatar']);
          UserPreferences.setUserAvatar(infoResponse['userAvatar']);
          Get.dialog(
            SuccessDialog(
              onPressed: () => Get.back(),
              message: infoResponse['message'],
              buttonTitle: 'OK',
            ),
          );
        }
      }
    } on dio.DioException catch (dioEx){
      String errMess = 'Có lỗi xảy ra nhưng không có mô tả chi tiết';
      if(dioEx.response != null){
        if(dioEx.response?.data is Map<String, dynamic>){
          errMess = dioEx.response?.data['message'];
        }
      }
      Get.dialog(
          NoVerifiedDialogs(message: errMess)
      );
    }catch(e){
      return;
    }finally{
      setIsLoading(false);
    }
  }

  Future<void> logout() async{
    /*final http = ApiEndpoints.makeNoSSLSecureRequest();
    var result = await http.post(
      Uri.parse(ApiEndpoints.logout),
      headers: {
        'Content-Type': 'application/json; charset=UTF-8',
        'Authorization': 'Bearer ${UserPreferences.getUserToken()}',
      },
    );

    if(result.statusCode == 200){
      UserPreferences.clearData();
      return true;
    }
    return false;*/
    setIsLoading(true);
    try{
      final DioClient dioClient = DioClient();
      var result = await dioClient.dio.post(ApiEndpoints.logout,
        options: Options(
            headers: {
              'Content-Type': 'application/json; charset=UTF-8',
            }
        ),);
      if(result.statusCode == 200){
        await UserPreferences.clearData();
        Get.delete<AddressController>();
        Get.delete<CartController>();
        Get.delete<OrderHistoryController>();
        /*Get.delete<ProductController>();*/
        Get.find<SaveController>().clearFavourite();
        Get.offAll(() => const LoginPage());
        /*Get.reset();*/
        return;
      }
    } on dio.DioException catch (dioEx){
      String errMess = 'Có lỗi xảy ra nhưng không có mô tả chi tiết';
      if(dioEx.response != null){
        if(dioEx.response?.data is Map<String, dynamic>){
          errMess = dioEx.response?.data['message'];
        }
      }
      Get.dialog(
          NoVerifiedDialogs(message: errMess)
      );
    }catch(e){
      return;
    }finally{
      setIsLoading(false);
    }
  }

  Future<void> changeUserPassword(Map<String,String> changePasswordModel) async{
    /*final http = ApiEndpoints.makeNoSSLSecureRequest();
    var result = await http.post(
      Uri.parse(ApiEndpoints.logout),
      headers: {
        'Content-Type': 'application/json; charset=UTF-8',
        'Authorization': 'Bearer ${UserPreferences.getUserToken()}',
      },
    );

    if(result.statusCode == 200){
      UserPreferences.clearData();
      return true;
    }
    return false;*/
    setIsLoading(true);
    try{
      final DioClient dioClient = DioClient();
      var result = await dioClient.dio.post(ApiEndpoints.changePassword,
          options: Options(
            headers: {
              'Content-Type': 'application/json; charset=UTF-8',
            },
          ),
          data: changePasswordModel
      );
      if(result.statusCode == 200){
        Get.dialog(
          SuccessDialog(
              onPressed: () => Get.back(),
              message: 'Cập nhật mật khẩu thành công!',
              buttonTitle: 'Ok')
        );
      }
    } on dio.DioException catch (dioEx){
      String errMess = 'Có lỗi xảy ra nhưng không có mô tả chi tiết';
      if(dioEx.response != null){
        if(dioEx.response?.data is Map<String, dynamic>){
          errMess = dioEx.response?.data['message'];
        }
      }
      Get.dialog(
          NoVerifiedDialogs(message: errMess)
      );
    }catch(e){
      return;
    }finally{
      setIsLoading(false);
    }
  }
}
