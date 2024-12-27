
import 'package:flutter/services.dart';

class ImageToByte{
  static Future<Uint8List> assetToXFile(String assetPath) async {
    // Tải hình ảnh từ assets
    ByteData data = await rootBundle.load(assetPath);
    return data.buffer.asUint8List();
  }
}
