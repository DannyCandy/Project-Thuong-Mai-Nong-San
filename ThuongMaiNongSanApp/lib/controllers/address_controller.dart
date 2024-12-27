import 'package:apptmns/logic_constants/api_endpoints.dart';
import 'package:apptmns/views/auth/dialogs/confirm_delete_address.dart';
import 'package:apptmns/views/auth/dialogs/info_dialogs.dart';
import 'package:apptmns/views/auth/dialogs/no_verified_dialogs.dart';
import 'package:dio/dio.dart';
import 'package:get/get.dart';

import '../services/dio_db_connect.dart';
import '../services/user_info_store.dart';
import '../views/auth/dialogs/success_dialogs.dart';

class AddressController extends GetxController {
  RxList<Map<String, dynamic>> listUserAddress = <Map<String, dynamic>>[].obs;
  RxBool isLoading = false.obs;

  @override
  Future<void> onInit() async {
    // TODO: implement onInit
    super.onInit();
    await fetchAddresses();
  }

  Future<void> onDeleteAddress(String idttlh) async {
    bool? result = await Get.dialog<bool>(
        const ConfirmDeleteAddressDialog(message: 'Bạn có chắc chắn muốn xóa?', buttonTitle: 'Đồng ý')
    );
    if(result != true) return;
    isLoading.value = true;
    try{
      final DioClient dioClient = DioClient();
      final response = await dioClient.dio.put(ApiEndpoints.deleteUserAddress,
          options: Options(
              headers: {
                'Content-Type': 'application/json; charset=UTF-8',
              }
          ),
        queryParameters: {
          'ttlhId': idttlh
        }
      );
      if(response.statusCode == 200){
        listUserAddress.removeWhere((i) => i['idttlh'] == idttlh);
        if(response.data['idttlh'].toString().isNotEmpty){
          resetDefaultPos(response.data['idttlh']);
        }
        Get.dialog(
          SuccessDialog(
            onPressed: () => Get.back(),
            message: 'Xóa địa chỉ thành công!',
            buttonTitle: 'OK',
          ),
        );
      }
    }on DioException catch (dioEx){
      String errMess = 'Có lỗi xảy ra nhưng không có mô tả chi tiết';
      if(dioEx.response != null){
        if(dioEx.response?.data is Map<String, dynamic>){
          errMess = dioEx.response?.data['message'];
        }
      }
      Get.dialog(
          NoVerifiedDialogs(message: errMess)
      );
      return;
    }catch(e){
      return;
    }finally{
      isLoading.value = false;
    }
    return;
  }

  Future<void> onUpdateAddress(Map<String, dynamic> updateAddress) async {
    isLoading.value = true;
    try{
      final DioClient dioClient = DioClient();
      final response = await dioClient.dio.put(ApiEndpoints.updateUserAddress,
        options: Options(
            headers: {
              'Content-Type': 'application/json; charset=UTF-8',
            }
        ),
        data: updateAddress,
      );
      if(response.statusCode == 200){
        int index = listUserAddress.indexWhere((i) => i['idttlh'] == updateAddress['idttlh']);
        listUserAddress[index] = updateAddress;
        if(response.data['idttlh'].toString().isNotEmpty){
          resetDefaultPos(response.data['idttlh']);
        }
        Get.dialog(
          SuccessDialog(
            onPressed: () => Get.back(),
            message: 'Cập nhật địa chỉ thành công!',
            buttonTitle: 'OK',
          ),
        );
      }
    }on DioException catch (dioEx){
      String errMess = 'Có lỗi xảy ra nhưng không có mô tả chi tiết';
      if(dioEx.response != null){
        if(dioEx.response?.data is Map<String, dynamic>){
          errMess = dioEx.response?.data['message'];
        }
      }
      Get.dialog(
          NoVerifiedDialogs(message: errMess)
      );
      return;
    }catch(e){
      return;
    }finally{
      isLoading.value = false;
    }
    return;
  }

  Future<void> onCreateAddress(Map<String, dynamic> createAddress) async {
    isLoading.value = true;
    try{
      final DioClient dioClient = DioClient();
      final response = await dioClient.dio.post(ApiEndpoints.addNewUserAddress,
        options: Options(
            headers: {
              'Content-Type': 'application/json; charset=UTF-8',
            }
        ),
        data: createAddress,
      );
      if(response.statusCode == 200){
        listUserAddress.add(createAddress);
        if(response.data['idttlh'].toString().isNotEmpty){
          resetDefaultPos(response.data['idttlh']);
        }
        Get.dialog(
          SuccessDialog(
            onPressed: () => Get.back(),
            message: 'Thêm địa chỉ thành công!',
            buttonTitle: 'OK',
          ),
        );
      }
    }on DioException catch (dioEx){
      String errMess = 'Có lỗi xảy ra nhưng không có mô tả chi tiết';
      if(dioEx.response != null){
        if(dioEx.response?.data is Map<String, dynamic>){
          errMess = dioEx.response?.data['message'];
        }
      }
      Get.dialog(
          NoVerifiedDialogs(message: errMess)
      );
      return;
    }catch(e){
      return;
    }finally{
      isLoading.value = false;
    }
    return;
  }

  Future<void> fetchAddresses() async {
    isLoading.value = true;
    try{
      final DioClient dioClient = DioClient();
      var result = await dioClient.dio.get(ApiEndpoints.getAllUserAddress,
          options: Options(
              headers: {
                'Content-Type': 'application/json; charset=UTF-8',
              }
          ),
          queryParameters: {
            'userId':UserPreferences.getUserId()
          }
      );
      if (result.statusCode == 200) {
        List<Map<String, dynamic>> data = List<Map<String, dynamic>>.from(result.data);
        for (var item in data) {
          if (item['defaultPos'] == true) {
            item['isChosen'] = true;
          } else {
            item['isChosen'] = false;
          }
        }

        listUserAddress.assignAll(data);
      }
    }on DioException catch (dioEx){
      String errMess = 'Có lỗi xảy ra nhưng không có mô tả chi tiết';
      if(dioEx.response != null){
        if(dioEx.response!.statusCode == 401){
          errMess = 'Trước hết, bạn cần đăng nhập';
        }
        if(dioEx.response?.data is Map<String, dynamic>){
          errMess = dioEx.response?.data['message'];
        }
      }
      Get.dialog(
          NoVerifiedDialogs(message: errMess)
      );
      return;
    }catch(e){
      return;
    }finally{
      isLoading.value = false;
    }
  }
  
  String getChosenUserAddressId(){
    Map<String,dynamic> chosenAddress = listUserAddress.firstWhere((address) => address['isChosen'] == true,
        orElse: () => {});
    if(chosenAddress.isNotEmpty){
      return chosenAddress['idttlh'];
    }
    return "";
  }

  void setChosen(String addressId){
    for (var item in listUserAddress) {
      if (item['idttlh'] == addressId) {
        item['isChosen'] = true;
      } else {
        item['isChosen'] = false;
      }
    }
    listUserAddress.refresh();
  }

  void resetDefaultPos(String posId) {
    if(listUserAddress.isEmpty){
      return;
    }
    int indexToTrue = listUserAddress.indexWhere((i) => i['idttlh'] == posId);
    // Nếu không tìm thấy index, thoát khỏi hàm
    if (indexToTrue == -1) {
      return;
    }
    // Cập nhật giá trị 'a' của tất cả phần tử
    for (int i = 0; i < listUserAddress.length; i++) {
      // Nếu phần tử có index khác indexToTrue, đặt 'a' thành false
      listUserAddress[i]['defaultPos'] = (i == indexToTrue);
    }
  }
}