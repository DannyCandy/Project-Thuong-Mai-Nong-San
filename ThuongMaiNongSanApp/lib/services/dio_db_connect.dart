import 'dart:io';

import 'package:apptmns/controllers/address_controller.dart';
import 'package:apptmns/controllers/order_history_controller.dart';
import 'package:apptmns/logic_constants/api_endpoints.dart';
import 'package:apptmns/services/user_info_store.dart';
import 'package:apptmns/views/auth/login_page.dart';
import 'package:dio/dio.dart';
import 'package:dio/io.dart';
import 'package:get/get.dart';

class DioClient {
  static final DioClient _instance = DioClient._internal(); // Singleton instance
  final Dio _dio = Dio();

  factory DioClient() {
    return _instance;
  }

  DioClient._internal() {
    // Cấu hình Dio (Interceptors, baseUrl, etc.)
    _dio.options.baseUrl = ApiEndpoints.apiUri; // Thay URL API của bạn
    _dio.options.connectTimeout = const Duration(seconds: 60); // Thời gian timeout

    // Thêm các Interceptor cho việc xác thực
    _dio.interceptors.add(
      InterceptorsWrapper(
        onRequest: (options, handler) async {
          // Lấy token từ SharedPreferences
          final accessToken = UserPreferences.getUserToken();
          if (accessToken != null && accessToken.isNotEmpty) {
            options.headers['Authorization'] = 'Bearer $accessToken';
          }
          return handler.next(options); // Tiếp tục với request

        },
        onResponse: (response, handler) {
          handler.next(response);
        },
        onError: (DioException e, handler) async {
          // Xử lý lỗi token hết hạn (401)
          if (e.response?.statusCode == 401) {
            final newToken = await _refreshToken();
            if (newToken != null) {
              //Neu lay duoc token
              final requestOptionsVar = e.requestOptions;
              requestOptionsVar.headers['Authorization'] = 'Bearer $newToken';
              try{
                /*final cloneRequest = await _dio.request(
                  requestOptionsVar.path,
                  options: Options(
                    method: requestOptionsVar.method,
                    headers: requestOptionsVar.headers,
                  ),
                  data: requestOptionsVar.data,
                  queryParameters: requestOptionsVar.queryParameters,
                );
                return handler.resolve(cloneRequest);*/

                //Cách 2:
                final requestAgain = await _dio.fetch(requestOptionsVar); // Sử dụng lại request gốc
                return handler.resolve(requestAgain);
              }catch(innerErr){
                Get.offAll(() => const LoginPage());
                return handler.reject(e);
              }
            }else{
              //Khong lay duoc thi login
              Get.delete<OrderHistoryController>();
              Get.offAll(() => const LoginPage());
              Get.delete<AddressController>();
              return handler.reject(e);
            }
          }
          return handler.next(e); // Tiếp tục xử lý lỗi nếu không làm mới token
        },
      ),
    );

    (_dio.httpClientAdapter as IOHttpClientAdapter).onHttpClientCreate = (HttpClient client) {
      client.badCertificateCallback = (X509Certificate cert, String host, int port) => true;
      return client;
    };
  }

  Dio get dio => _dio;

  Future<String?> _refreshToken() async {
    if(UserPreferences.getUserToken() == null || UserPreferences.getUserToken()!.isEmpty){
      return null;
    }
    final Map<String, String> resetTokenData = {
      'accessToken':UserPreferences.getUserToken()!,
      'refreshToken':UserPreferences.getUserRefreshToken()!
    };
    try {
      final result = await _dio.post(ApiEndpoints.refreshToken, data: resetTokenData);
      if (result.statusCode == 200) {
        final resData = result.data;
        final newAccessToken = resData['accessToken'];
        UserPreferences.setUserToken(newAccessToken);
        final refreshToken = resData['refreshToken'];
        UserPreferences.setUserRefreshToken(refreshToken);
        return newAccessToken;
      } else if (result.statusCode == 400 || result.statusCode == 401) {
        return null;
      }
    }on DioException catch (e){
      return null;
    }catch(e){
      return null;
    }
    return null;
  }
}
