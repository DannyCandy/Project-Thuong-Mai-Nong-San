
import 'package:apptmns/models/order_history.dart';
import 'package:apptmns/services/user_info_store.dart';
import 'package:dio/dio.dart';
import 'package:get/get.dart';

import '../logic_constants/api_endpoints.dart';
import '../services/dio_db_connect.dart';
import '../views/auth/dialogs/no_verified_dialogs.dart';

class OrderHistoryController extends GetxController {
  RxList<OrderHistory> listOrderHistory = <OrderHistory>[].obs;
  RxInt currentPage = 1.obs;

  Map<String,String> getTotalAmountAndMethodPaidFromOrderId(String orderId){
    OrderHistory orderHistory = listOrderHistory.firstWhere((i) => i.orderId == orderId);
    return {
      'totalAmount':orderHistory.totalPrice.toString(),
      'methodPaid': orderHistory.paymentMethod!,
    };
  }

  /*Future<void> getAllOrder() async {
    try{
      if(UserPreferences.getUserId() == null){
        Get.offAll(() => const LoginPage());
        return ;
      }
      final DioClient dioClient = DioClient();
      var result = await dioClient.dio.get(ApiEndpoints.getOrderHistory,
        options: Options(
            headers: {
              'Content-Type': 'application/json; charset=UTF-8',
            }
        ),
        queryParameters: {
          'userId':UserPreferences.getUserId(),
          'pageSize':5,
          'pageIndex':1,
          'month':-1,
          'year':-1,
        }
      );

      if(result.statusCode == 200){
        final jsonOrdersData = result.data;
        final List<Map<String, dynamic>> tempData = (jsonOrdersData as List).cast<Map<String, dynamic>>();

        listOrderHistory.assignAll(tempData.map((json) => OrderHistory.fromJson(json)).toList());
        return;
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
    }
  }*/

  Future<bool> canGetMoreOrderHistory({required int pageIndex, int month = -1,
    int year = -1}) async {
    try{
      final DioClient dioClient = DioClient();
      var result = await dioClient.dio.get(ApiEndpoints.getOrderHistory,
          options: Options(
            headers: {
              'Content-Type': 'application/json; charset=UTF-8',
            },
          ),
          queryParameters: {
            'userId':UserPreferences.getUserId(),
            'pageSize':5,
            'pageIndex':pageIndex,
            'month':month,
            'year':year,
          }
      );

      if(result.statusCode == 200){
        final List<Map<String, dynamic>> tempData = (result.data as List).cast<Map<String, dynamic>>();

        if(tempData.isEmpty){
          return false;
        }
        listOrderHistory.addAll(
            tempData.map((json) => OrderHistory.fromJson(json)).toList()
        );
        return true;
      }
      return false;
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
      return false;
    }catch(e){
      return false;
    }
  }

  List<OrderDetails> getOrderDetailFromOrderId(String orderId){
    OrderHistory orderReturned = listOrderHistory.firstWhere((i) => i.orderId == orderId);
    List<OrderDetails> listOrderDetails = orderReturned.orderDetails!;
    return listOrderDetails;
  }

  void clearListOrderHistory(){
    listOrderHistory.clear();
    currentPage.value = 1;
  }

}
