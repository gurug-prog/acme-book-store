import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class FileServiceService {
  public saveBase64File(base64Content: string, filename: string): void {
    const blob = this.base64toBlob(base64Content);
    this.saveFile(blob, filename);
  }

  public saveFile(blob: Blob, filename: string): void {
    var a = document.createElement("a");
    a.href = URL.createObjectURL(blob);
    a.download = filename;
    a.click();
    a.remove();
  }

  public openBase64InNewTab(base64Content: string, contentType: string): void {
    const blob = this.base64toBlob(base64Content, contentType);
    this.openFileInNewTab(blob);
  }

  public openFileInNewTab(blob: Blob): void {
    var a = document.createElement("a");
    a.href = URL.createObjectURL(blob);
    a.target = "_blank";
    a.click();
    a.remove();
  }

  public base64toBlob(
    b64Data: string,
    contentType: string = "",
    sliceSize: number = 512
  ): Blob {
    const byteCharacters = atob(b64Data);
    const byteArrays = [];

    for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
      const slice = byteCharacters.slice(offset, offset + sliceSize);
      const byteNumbers = new Array(slice.length);
      for (let i = 0; i < slice.length; i++) {
        byteNumbers[i] = slice.charCodeAt(i);
      }
      const byteArray = new Uint8Array(byteNumbers);
      byteArrays.push(byteArray);
    }

    const blob = new Blob(byteArrays, { type: contentType });
    return blob;
  }

  public fileToBase64(file: File): Promise<string> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () =>
        resolve(this.dataUrlToBase64(reader.result as string));
      reader.onerror = reject;
    });
  }

  public fileToDataUrl(file: File): Promise<string> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () =>
        resolve(reader.result as string);
      reader.onerror = reject;
    });
  }

  public dataUrlToBase64(dataUrl: string): string {
    const headerEndIndex = dataUrl.indexOf(",") + 1;
    if (headerEndIndex > 0) {
      return dataUrl.substring(headerEndIndex);
    }

    return dataUrl;
  }

  public base64ToDataURL(contentType: string, base64: string): string {
    return `data:${contentType};base64,${base64}`;
  }

  public dataURLtoFile(dataurl: string, filename: string): File | null {
    const arr = dataurl.split(",");
    const mime = arr[0].match(/:(.*?);/)![1];
    const bstr = atob(arr[arr.length - 1]);
    let n = bstr.length;
    const u8arr = new Uint8Array(n);

    while (n--) {
      u8arr[n] = bstr.charCodeAt(n);
    }

    try {
      return new File([u8arr], filename, { type: mime });
    } catch (error) {
      console.error("Error creating File object:", error);
      return null;
    }
  }

  public blobToDataURL(blob: Blob): Promise<string> {
    return new Promise((resolve, reject) => {
      const reader = new FileReader();
      reader.onloadend = () => resolve(reader.result as string);
      reader.onerror = reject;
      reader.readAsDataURL(blob);
    });
  }
}
