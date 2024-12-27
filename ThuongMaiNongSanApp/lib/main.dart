import 'dart:io';

import 'package:apptmns/services/user_info_store.dart';
import 'package:get/get_navigation/get_navigation.dart';
import 'core/themes/app_themes.dart';
import 'core/routes/app_routes.dart';
import 'core/routes/on_generate_route.dart';
import 'package:flutter/material.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  await UserPreferences.init();
  HttpOverrides.global = MyHttpOverrides();
  runApp(const MyApp());
}

class MyHttpOverrides extends HttpOverrides{
  @override
  HttpClient createHttpClient(SecurityContext? context){
    return super.createHttpClient(context)
      ..badCertificateCallback = (X509Certificate cert, String host, int port)=> true;
  }
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return GetMaterialApp(
      title: 'eGrocery',
      debugShowCheckedModeBanner: false,
      theme: AppTheme.defaultTheme,
      onGenerateRoute: RouteGenerator.onGenerate,
      initialRoute: AppRoutes.entryPoint,
    );
  }
}

