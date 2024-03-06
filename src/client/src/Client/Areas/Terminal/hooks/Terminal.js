import { Terminal as XtermTerminal } from 'xterm';
import { FitAddon } from '@xterm/addon-fit';
import {
  write,
  read,
  readCallback,
  create,
  dispose,
} from '../services';
import 'xterm/css/xterm.css';

export class Terminal {
  constructor({
    type,
    containerId,
    height,
    width,
    backgroundColor,
    foregroundColor,
    terminalId,
  }) {
    // default
    this.type = type;
    this.backgroundColor = backgroundColor;
    this.foregroundColor = foregroundColor;
    this.height = height;
    this.width = width;
    this.containerId = containerId;
    this.terminalId = terminalId;

    // essential
    this.pid = null;
    this.xterm = null;
  }

  /**
   * Private helper functions
   */
  #attachToDOM() {
    const domId = this.terminalId;

    // remove chiildren from the container
    this.#detachFromDOM();

    // add terminal element to the container
    const newChild = document.createElement('div');
    newChild.setAttribute('id', domId);
    newChild.style.height = this.height;
    newChild.style.width = this.width;
    document.getElementById(this.containerId)?.appendChild(newChild);

    // open terminal element and fit it to the container
    const fitAddon = new FitAddon();
    this.xterm.loadAddon(fitAddon);
    this.xterm.open(document.getElementById(domId));
    fitAddon.fit();
  }

  #detachFromDOM() {
    document.getElementById(this.containerId)?.replaceChildren();
  }

  async #setPtyWriteEvent() {
    if (this.pid) {
      this.xterm.onData((data) => write(this.pid, data));
    }
  }

  async #setPtyReadEvent() {
    if (this.pid) {
      const readChannel = this.pid.toString(); // we will be using the pid as the buffer name
      read(this.pid, readChannel); // read data into buffer
      readCallback(readChannel, (data) => { this.xterm.write(data); }); // receive data from buffer
    }
  }

  async #createPty() {
    return create({
      type: this.type,
      cols: this.xterm.cols,
      rows: this.xterm.rows,
    });
  }

  async #registerPtyEvents() {
    this.#setPtyWriteEvent();
    this.#setPtyReadEvent();
  }

  #createXterm() {
    return new XtermTerminal({
      theme: {
        background: this.backgroundColor,
        foreground: this.foregroundColor,
        cursor: this.foregroundColor,
      },
    });
  }

  /**
   * Public functions
   */
  async init() {
    if (!this.exists()) {
      this.xterm = this.#createXterm(); // terminal must be created first
      this.#attachToDOM(); // must be attached before we create pty
      this.pid = await this.#createPty(); // must be creaeted before register
      this.#registerPtyEvents();
    }
  }

  dispose() {
    this.xterm?.dispose();
    dispose(this.pid);
  }

  open() {
    if (this.exists()) {
      this.#attachToDOM();
    }
  }

  clear() {
    if (this.exists()) {
      this.#detachFromDOM();
    }
  }

  destroy() {
    if (this.exists()) {
      this.dispose();
      this.clear();
    }
  }

  exists() {
    const pidExists = this.pid !== undefined && this.pid !== null;
    const xtermExists = this.xterm !== undefined && this.xterm !== null;

    return pidExists && xtermExists;
  }

  getPidType() {
    return this.pid && this.type ? `${this.pid} - ${this.type}` : '';
  }
}
