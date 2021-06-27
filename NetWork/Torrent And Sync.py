from typing import Sequence, Union
import wmi
import subprocess
import glob
import re
import pywinauto
import pywin
import psutil
import keyboard
processes = [p for p in wmi.WMI().Win32_Process() if p.Name in ["GoogleDriveFS.exe",
                                                                "googledrivesync.exe", "qbittorrent.exe", "Surfshark.exe", "mstsc.exe"]]

def rdp(start_rdp_software: bool = True):
    '''rdp detects if the rdp software is running on the computer. if the start_rdp_software is true at calling, the function will start the rdp if there is no program that runs.
    elsewise, it will close it

    :param start_rdp_software: switch. it can be called to run(true) or stop(false) the rdp software, defaults to True
    :type start_rdp_software: bool, optional
    '''    
    if start_rdp_software:
        running = False
        for process in processes:
            if process.CommandLine == '"mstsc.exe" "D:\\Drive\\מסמכים\\vms\\משרדוש.rdp"' or process.CommandLine == '"C:\\Windows\\System32\\mstsc.exe" /v:"132.64.161.2"':
                running = True
        if not running:
            subprocess.Popen('"mstsc.exe" "D:\\Drive\\מסמכים\\vms\\משרדוש.rdp"',
                             creationflags=subprocess.CREATE_NO_WINDOW, stdout=subprocess.PIPE, shell=True)
    else:
        for process in processes:
            if process.CommandLine == '"mstsc.exe" "D:\\Drive\\מסמכים\\vms\\משרדוש.rdp"' or process.CommandLine == '"C:\\Windows\\System32\\mstsc.exe" /v:"132.64.161.2"':
                try:
                    process.Terminate()
                except:
                    continue       

def allowd_macs(mac_address: Union[dict, tuple, str]):
    allowed = ['00-E0-4C-68-13-E4']
    if isinstance(mac_address, str):
        if mac_address in allowed:
            return True
    for allowed_elements in allowed:
        if allowed_elements in mac_address:
            return True
    return False


def start_process(process_list: list):
    start = {
        "Surfshark.exe": 'C:/Program Files (x86)/Surfshark/Surfshark.exe',
        'qbittorrent.exe': "C:/Program Files/qBittorrent/qbittorrent.exe", "GoogleDriveFS.exe": glob.glob(
            'C:/Program Files/Google/Drive File Stream/*/GoogleDriveFS.exe', recursive=True)[0],
        "googledrivesync.exe": 'C:/Program Files/Google/Drive/googledrivesync.exe'
        }
    for p in process_list:
        start_this_process = False
        for k in processes:
            if p == k.Name:
                start_this_process = True
        if not start_this_process:
            subprocess.Popen(start[p], shell=True, creationflags=subprocess.CREATE_NO_WINDOW, stdout=subprocess.PIPE)


def stop_process(process_list: list):
    for proc in process_list:
        for k in processes:
            if proc == k.Name:
                try:
                    k.Terminate()
                except:
                    continue


def disallow(process_list: list = ["GoogleDriveFS.exe", "googledrivesync.exe"],
             mute: bool = True, run_process: list = None):
    stop_process(process_list)
    if run_process:
        start_process(run_process)
    if mute:
        subprocess.run(["powershell", "-command", "Set-AudioDevice -PlaybackMute $true"],
                       shell=True, creationflags=subprocess.CREATE_NO_WINDOW)
    else:
        subprocess.run(["powershell", "-command", "Set-AudioDevice -PlaybackMute $false"],
                       shell=True, creationflags=subprocess.CREATE_NO_WINDOW)


def allow(process_list: list = ["Surfshark.exe", 'qbittorrent.exe'],  unmute: bool = True):
    start_process(process_list)
    if unmute:
        subprocess.run(["powershell", "-command", "Set-AudioDevice -PlaybackMute $false"],
                       shell=True, creationflags=subprocess.CREATE_NO_WINDOW)
    else:
        subprocess.run(["powershell", "-command", "Set-AudioDevice -PlaybackMute $true"],
                       shell=True, creationflags=subprocess.CREATE_NO_WINDOW)


def _actiavte_rasdial():
    pywinauto.findwindows.find_window()


def dial_connect(connection: str = 'Connected to\nek\nCommand completed successfully.\n'):
    connected = False if connection == 'Connected to\nek\nCommand completed successfully.\n' else True
    if connected:
        subprocess.Popen(
            'rasphone ek',
            shell=True, creationflags=subprocess.CREATE_NO_WINDOW, stdout=subprocess.PIPE)
        # _actiavte_rasdial()
        # keyboard.press_and_release('alt')
        keyboard.press_and_release('enter')
        # _actiavte_rasdial()
        keyboard.press_and_release('enter')
        subprocess.Popen(["powershell", "-command", "Set-AudioDevice -PlaybackMute $false"],
                         shell=True, creationflags=subprocess.CREATE_NO_WINDOW, stdout=subprocess.PIPE)

def _hiercy(network_status: dict):
    if network_status["ek"]:
        return "ek"
    elif network_status["good_lan"]:
        return 'good_lan'
    elif network_status["lan"]:
        return "lan"
    return "Wi-Fi"


def get_net_status(supreme_mac: Union[str, list, tuple, set] = '00-E0-4C-68-13-F8'):

    nets, stats = psutil.net_if_addrs(), psutil.net_if_stats()
    networks = subprocess.Popen('pwsh -executionPolicy bypass -command "Get-NetConnectionProfile"',
                                shell=True, stdout=subprocess.PIPE).communicate()[0].decode()
    macs = {nets[key][0][1]: key.strip('\u200f') for key in nets if stats[key][0]}
    lan_mac = get_lan(supreme_mac, macs)
    profiles = [Net[19:].strip('\u200f') for net in networks.split('\r\n\r\n')
                for Net in net.split('\r\n') if 'InterfaceAlias   :' in Net]
    wifi = [net[15:].split('\r\n')[0] for net in networks.split('Name') if 'Wi-Fi' in net]
    wifi = wifi[0] if wifi else None
    vpn = [key.strip('\u200f') for key in nets if stats[key][0] and re.search(r'(surf|shark)', key)]
    return {"Wi-Fi": wifi, "lan": (lan_mac), "ek": ("ek" in profiles), "vpn": vpn, "good_lan": allowd_macs(macs)} if vpn else {"Wi-Fi": wifi, "lan": (lan_mac), "ek": ("ek" in profiles), "good_lan": allowd_macs(macs)}


def get_lan(macs: Union[str, list, tuple, set] = '00-E0-4C-68-13-F8', dict_of_macs: dict = None):
    SMAC = ['00-E0-4C-68-13-F8', "00-FF-33-C8-0F-88"]
    if isinstance(macs, str) and macs in dict_of_macs:
        return True
    elif not isinstance(macs, str):
        for m in macs:
            if m in SMAC:
                return True
    return False


def get_status(supreme_mac: Union[str, list, tuple, set] = '00-E0-4C-68-13-F8'):
    wifi = {
        "allowed":
        ["Oliver", "Oliver5", "Oliver", "x018_497622", "TNCAPE5A34D", "MSBR", "Azrieli_Modiin_WIFI", "lu shalmata",
         "mickey", "Mickey", "Network", "192.168.1.", "Silmarill", "wintunshark0", "saret", "Saret", "huji-meonot"],
        "disallowed": ["HUJI-netX", "eduroam", "HUJI-guest", "132.64"]}
    network_status = get_net_status(supreme_mac)
    hierarchy = _hiercy(network_status)
    if hierarchy == "Wi-Fi":
        current_network = network_status["Wi-Fi"].split(' ')[0]
        w = 0 if re.search(current_network, " ".join(wifi["disallowed"])) else 1
        return {"Wi-Fi": w, "vpn": network_status["vpn"]} if network_status.get("vpn") else {"Wi-Fi": w}
    elif hierarchy == 'good_lan':
        return {'good_lan': True}
    elif hierarchy == "lan":
        return {"lan": network_status["lan"], "vpn": network_status["vpn"]} if network_status.get("vpn") else {"lan": network_status["lan"]}
    return {"ek": network_status["ek"], "vpn": network_status["vpn"]} if network_status.get("vpn") else {"ek": network_status["ek"]}


def do_the_schtik(supreme_mac: str = '00-E0-4C-68-13-F8', discontinue: str = None):
    network_status = get_status(supreme_mac)
    if network_status.get("vpn"):
        allow(["qbittorrent.exe", "GoogleDriveFS.exe", "googledrivesync.exe", "Surfshark.exe"])
        rdp(False)
        if network_status.get("lan"):
            dial_connect(discontinue)
        elif network_status.get('Wi-Fi') and network_status['Wi-Fi'] == 0:
            allow(["qbittorrent.exe", "GoogleDriveFS.exe", "googledrivesync.exe", "Surfshark.exe"], False)
    elif network_status.get('Wi-Fi') and network_status['Wi-Fi'] == 1:
        allow(["qbittorrent.exe", "GoogleDriveFS.exe", "googledrivesync.exe", "Surfshark.exe"])
        rdp(False)
    elif network_status.get("good_lan") and network_status["good_lan"]:
        allow(["qbittorrent.exe", "GoogleDriveFS.exe", "googledrivesync.exe", "Surfshark.exe"])
        rdp(False)
    else:
        if network_status.get("lan"):
            dial_connect(discontinue)
            disallow(["Surfshark.exe", 'qbittorrent.exe'], False,
                     run_process=["GoogleDriveFS.exe", "googledrivesync.exe"])
            rdp()
        elif network_status.get("ek"):
            disallow(["Surfshark.exe", 'qbittorrent.exe'], False,
                     run_process=["GoogleDriveFS.exe", "googledrivesync.exe"])
            rdp()
        else:
            disallow(["Surfshark.exe", 'qbittorrent.exe'], run_process=["GoogleDriveFS.exe", "googledrivesync.exe"])


def main():
    discontinue = subprocess.Popen(
        'rasdial',
        shell=True, creationflags=subprocess.CREATE_NO_WINDOW, stdout=subprocess.PIPE).communicate()[0].decode().split(
        "\r\n")[0]
    do_the_schtik(discontinue=discontinue)

    exit()


if __name__ == '__main__':

    main()
    # get_net_status()
