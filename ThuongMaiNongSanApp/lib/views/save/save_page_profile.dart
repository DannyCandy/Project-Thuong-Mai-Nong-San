import 'package:apptmns/controllers/save_controller.dart';
import 'package:apptmns/views/save/inner_save_page.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

import 'empty_save_page.dart';

class SavePageProfile extends StatelessWidget {
  const SavePageProfile({
    super.key,
  });

  @override
  Widget build(BuildContext context) {

    Get.lazyPut(()=>SaveController());
    final saveController = Get.find<SaveController>();

    return Obx((){
      if(saveController.listSavedItem.isEmpty){
        return const Scaffold(
            body: EmptySavePage()
        );
      }
      return const InnerSavePage();
    });
  }
}