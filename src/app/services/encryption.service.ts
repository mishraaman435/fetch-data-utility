import { Injectable } from '@angular/core';
import * as CryptoJS from 'crypto-js';


@Injectable({
  providedIn: 'root'
})
export class EncryptionService {

  private apiKey = '8hJd^mL2kQf6zU!vN3pR$bX5aWnYr7sC'; 

  encryptData(data: any): string {
    const key = CryptoJS.enc.Utf8.parse(this.apiKey);
    const iv = CryptoJS.lib.WordArray.random(16); 

    const encrypted = CryptoJS.AES.encrypt(JSON.stringify(data), key, {
      iv: iv,
      padding: CryptoJS.pad.Pkcs7,
      mode: CryptoJS.mode.CBC
    });

    return iv.toString(CryptoJS.enc.Base64) + ':' + encrypted.toString();
  }

  decryptData(encryptedData: string): any {
    const parts = encryptedData.split(':');
    const iv = CryptoJS.enc.Base64.parse(parts[0]);
    const cipherText = parts[1];
    const key = CryptoJS.enc.Utf8.parse(this.apiKey);

    const decrypted = CryptoJS.AES.decrypt(cipherText, key, {
      iv: iv,
      padding: CryptoJS.pad.Pkcs7,
      mode: CryptoJS.mode.CBC
    });

    return JSON.parse(decrypted.toString(CryptoJS.enc.Utf8));
  }
}
